using System;
using EcommerceDataAnalysisToolServer.Models;
using EcommerceDataAnalysisToolServer.Data;
using EcommerceDataAnalysisToolServer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace EcommerceDataAnalysisToolServer.Repository
{
    public class SalesDataAnalysisRepository : ISalesDataAnalysisRepository
    {
        private DataContext _context;

        public SalesDataAnalysisRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Save the changes to the database
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            int saved = _context.SaveChanges();
            return saved == 1;
        }

        /// <summary>
        /// Method that handles Get all data
        /// </summary>
        /// <returns>return all the data from the DB</returns>
        public ICollection<Ecommerce> GetAllSales()
        {
            return _context.Ecommerce.ToList();
        }

        /// <summary>
        /// Method to Get the sales data by ID
        /// </summary>
        /// <param name="id">sales data id</param>
        /// <returns>sales data of the given id</returns>
        public Ecommerce GetSalesById(int id)
        {
            return _context.Ecommerce.Where(sale => sale.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Method for checking whether the sales data exist for the given id
        /// </summary>
        /// <param name="id">sales data id</param>
        /// <returns></returns>
        public bool SalesDataExists(int id)
        {
            return _context.Ecommerce.Any(sales => sales.Id == id);
        }

        /// <summary>
        /// Method to handle create new data.
        /// </summary>
        /// <param name="saleData">new sales data</param>
        /// <returns>true if new data has been created or false if new data is not created</returns>
        public bool AddSalesData(Ecommerce saleData)
        {
            saleData.Date = DateTime.UtcNow;
            _context.Add(saleData);
            return Save();
        }

        /// <summary>
        /// Method that handle updating a data
        /// </summary>
        /// <param name="id">sales data id</param>
        /// <param name="updatedSaleData">updated sales data</param>
        /// <returns>true if new data has been updated or false if new data is not updated</returns>
        public bool EditSalesData(Ecommerce updatedSaleData)
        {
            _context.Update(updatedSaleData);
            return Save();
        }

        /// <summary>
        /// Method that handle deleting a data
        /// </summary>
        /// <param name="id">sales data id</param>
        /// <returns>true if new data has been deleted or false if new data is not deleted</returns>
        public bool DeleteSalesData(int id)
        {
            _context.Remove(GetAllSales().FirstOrDefault(a => a.Id == id));
            return Save();
        }

        /// <summary>
        /// Method that calculate total revenue for the given year
        /// </summary>
        /// <param name="year">Year</param>
        /// <returns>Total Revenue for the given year</returns>
        public double GetTotalRevenueForYear(int year)
        {
            var startDate = new DateTime(year, 1, 1);
            var endDate = startDate.AddYears(1);

            var totalSales = _context.Ecommerce
                .Where(s => s.Date >= startDate && s.Date < endDate)
                .Sum(s => s.Price);

            return System.Math.Truncate(totalSales);
        }

        /// <summary>
        /// Method that calculate total sales count for the given month in the year
        /// </summary>
        /// <param name="month">month in MM format</param>
        /// <param name="year">Year</param>
        /// <returns>Total Revenue for the given year</returns>
        public double GetTotalSalesRecordInMonthForYear(int month, int year)
        {
            var saleInMonth = _context.Ecommerce
                                    .Where(s => s.Date.Year == year && s.Date.Month == month)
                                    .Count();
            return saleInMonth;
        }

        /// <summary>
        /// Method to get the category which has the highest sales in a year or month
        /// </summary>
        /// <param name="year">Year</param>
        /// <returns>name of the category</returns>
        public CategoryData GetCategoryWhichHasHighestSales(int year, int month)
        {
            var startDate = new DateTime();
            var endDate = new DateTime();
            var season = "";
            CategoryData seasonAndCategory = new CategoryData();

            if (month > 0)
            {
                startDate = new DateTime(year, month, 1);
                endDate = startDate.AddMonths(1);
                if (month == 5 || month == 6)
                {
                    season = "spring sale";
                }
                else if (month > 10 && month <= 12)
                {
                    season = "Holiday Sale";
                }
                else if (month == 1)
                {
                    season = "New year sale";
                }
            }
            else
            {
                startDate = new DateTime(year, 1, 1);
                endDate = startDate.AddYears(1);
            }


            var maxSales = _context.Ecommerce
               .Where(s => s.Date >= startDate && s.Date < endDate)
              .Max(s => s.Price);

            Ecommerce category = _context.Ecommerce.Where(s => s.Price == maxSales).FirstOrDefault();
            seasonAndCategory.category = category.ProductCategory;
            if (season != "")
                seasonAndCategory.season = season;
           
            return seasonAndCategory;
        }

        /// <summary>
        /// Method that get most sold product in a year
        /// </summary>
        /// <param name="year">year in YYYY format</param>
        /// <returns>most sold product in given year</returns>
        public async Task<Ecommerce> GetHighestSoldProductForYear(int year)
        {

            var startDate = new DateTime(year, 1, 1);
            var endDate = startDate.AddYears(1);
            var query = _context.Ecommerce
                .Where(s => s.Date >= startDate && s.Date < endDate)
                .GroupBy(x => x.ProductName)
                .Select(g => new
                {
                    ProductName = g.Key,
                    TotalPrice = g.Sum(x => x.Price)
                })
                .OrderByDescending(x => x.TotalPrice)
                .FirstOrDefault();

            if (query == null)
            {
                return null;
            }

            var result = await _context.Ecommerce
                .FirstOrDefaultAsync(x => x.ProductName == query.ProductName);

            return result;
        }

        /// <summary>
        /// Method that get most sold product in a month for the given year
        /// </summary>
        /// <param name="month">month in MM format</param>
        /// <param name="year">year in MM format</param>
        /// <returns>most sold product in a month</returns>
        public async Task<List<Ecommerce>> GetSalesForMonthAndYearAsync(int month, int year)
        {
            return await _context.Set<Ecommerce>()
                                    .Where(s => s.Date.Year == year && s.Date.Month == month)
                                    .ToListAsync();
        }

        /// <summary>
        /// Method to get the total sale 
        /// </summary>
        /// <returns>return the total sale amount from the DB</returns>
        public double GetTotalSales()
        {
            var totalSales = _context.Ecommerce
                .Sum(s => s.Price);

            return totalSales;
        }

        /// <summary>
        /// Method to get the total no of records sale 
        /// </summary>
        /// <returns>return the total no of sale record for the particular year from the DB</returns>
        public double GetTotalCountSales(int year)
        {
            var startDate = new DateTime(year, 1, 1);
            var endDate = startDate.AddYears(1);

            var totalSalesCount = _context.Ecommerce
                .Where(s => s.Date >= startDate && s.Date < endDate)
                .Count();

            return totalSalesCount;
        }

        /// <summary>
        /// Method to get the sales in the particular month of the given year
        /// </summary>
        /// <param name="month">month in MM format</param>
        /// <param name="year">year in YYYY format</param>
        /// <returns>sales from the particular month of the given year</returns>
        public double GetAverageSalesForMonth(int month, int year)
        {
            var saleInMonth = _context.Ecommerce
                                    .Where(s => s.Date.Year == year && s.Date.Month == month)
                                    .Sum(s => s.Price);
            return saleInMonth;
        }
        /// <summary>
        /// Method to get all the sales data summary for the given year
        /// </summary>
        /// <param name="year">year in YYYY format</param>
        /// <returns>Sales data summary</returns>
        public async Task<SalesSummary> GetFilterBaseOnYear(int year)
        {
            double totalRevenue = GetTotalRevenueForYear(year);
            CategoryData categoryWithHighestSales = GetCategoryWhichHasHighestSales(year, 0);
            double totalSalesInYear = GetTotalRevenueForYear(year);
            double totalSales = GetTotalSales();
            string averageSaleInYear = (totalSalesInYear / totalSales).ToString("0.00");

            // add code for highest sold product
            // Task<Ecommerce> highestSaledProduct = GetHighestSoldProductForYear(year);

            var salesSummary = new SalesSummary
            {
                TotalRevenue = totalRevenue,
                CategoryWithHighestSales = categoryWithHighestSales,
                AverageSaleInYear = averageSaleInYear
            };

            return salesSummary;
        }

        /// <summary>
        /// 
        /// Get Prediction For Category
        /// </summary>
        /// <returns></returns>
        public CategoryData GetPredictionForCategory()
        {
            CategoryData[] data = new CategoryData[3];
            CategoryData results = new CategoryData();
            int year = 2020;
           
            for (int i=0;i<3;i++)
            {

                data[i]=GetCategoryWhichHasHighestSales(year, 0);
                year++;
               
            }

            
            int n = data.Length;
            int maxCount = 1;
            string prediction = data[0].category;
            int currCount = 1;
          
            for (int i = 0; i < n - 1; i++)
            {
                currCount = 1;
                for (int j = i + 1; j < n; j++)
                {
                    if (data[i].category == data[j].category)
                    {
                        currCount++;
                    }
                }
                if (currCount > maxCount)
                {
                    maxCount = currCount;
                    prediction = data[i].category;
                }
            }

            
           
            results.predictedCategory = prediction;

            return results;
        }

        /// <summary>
        /// Get Prediction For Category for a Month
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public CategoryData GetPredictionForCategoryOnMonth(int year)
        {
            CategoryData[] data = new CategoryData[12];
            CategoryData results = new CategoryData();
                    
            int month = 1;

            
            for (int i = 0; i < 12; i++)
            {

                data[i] = GetCategoryWhichHasHighestSales(year, month);
               
                month++;
              
            }
            int n = data.Length;
            int maxCount = 1;
            string prediction = data[0].category;
            int currCount = 1;
            for (int i = 0; i < n - 1; i++)
            {
                currCount = 1;
                for (int j = i + 1; j < n; j++)
                {
                    if (data[i].category == data[j].category)
                    {
                        currCount++;
                    }
                }
                if (currCount > maxCount)
                {
                    maxCount = currCount;
                    prediction = data[i].category;
                }
            }
           
           
            results.predictedCategory = prediction;

            return results;
           }

        /// <summary>
        /// GetPredictionForRevenue
        /// </summary>
        /// <returns></returns>
        public CategoryData GetPredictionForRevenue()
        {
            int year = 2020;
            CategoryData results = new CategoryData();
            double[] data = new double[3];
            double total = 0;
            for (int i=0;i<3;i++)
            {
                data[i] = GetTotalRevenueForYear(year);
                year++;
                total = total + data[i];
            }

            results.predictedRevenue = total / 3;

            return results;
        }

        /// <summary>
        /// GET Sales Forecast from the past sales analysis
        /// </summary>
        /// <returns></returns>
        public string GetPredictionData()
        {
            JObject prediction = new JObject();

            // Get prediction for category
            CategoryData[] categoryData = new CategoryData[3];
            int year = 2020;
            for (int i = 0; i < 3; i++)
            {
                categoryData[i] = GetCategoryWhichHasHighestSales(year, 0);
                year++;
            }
            int n = categoryData.Length;
            int maxCount = 1;
            string predictedCategory = categoryData[0].category;
            int currCount = 1;
            for (int i = 0; i < n - 1; i++)
            {
                currCount = 1;
                for (int j = i + 1; j < n; j++)
                {
                    if (categoryData[i].category == categoryData[j].category)
                    {
                        currCount++;
                    }
                }
                if (currCount > maxCount)
                {
                    maxCount = currCount;
                    predictedCategory = categoryData[i].category;
                }
            }
            prediction["predictedCategory"] = predictedCategory;

            // Get prediction for category on month
            //JObject monthlyPrediction = new JObject();
            //year = 2020;
            //for (int month = 1; month <= 12; month++)
            //{
            //    CategoryData data = GetCategoryWhichHasHighestSales(year, month);
            //    if (!monthlyPrediction.ContainsKey(data.category))
            //    {
            //        monthlyPrediction[data.category] = 1;
            //    }
            //    else
            //    {
            //        monthlyPrediction[data.category] = (int)monthlyPrediction[data.category] + 1;
            //    }
            //}
            //predictedCategory = monthlyPrediction.Properties().OrderByDescending(p => p.Value).First().Name;
            prediction["predictedCategoryOnMonth"] = predictedCategory;

            // Get prediction for revenue
            double[] revenueData = new double[3];
            double totalRevenue = 0;
            year = 2020;
            for (int i = 0; i < 3; i++)
            {
                revenueData[i] = GetTotalRevenueForYear(year);
                year++;
                totalRevenue += revenueData[i];
            }
            double predictedRevenue = totalRevenue / 3;
            prediction["predictedRevenue"] = System.Math.Truncate(predictedRevenue);

            // Return JSON string
            return prediction.ToString();
        }


        /// <summary>
        /// Method to search the sales data by product name
        /// </summary>
        /// <param name="productName">sales data by productName</param>
        /// <returns>sales data of the given product name</returns>
        public IQueryable<Ecommerce> SearchSalesByProductName(string productName)
        {
            IQueryable<Ecommerce> ecommerces = _context.Ecommerce.Where(sale => sale.ProductName.StartsWith(productName));
            return ecommerces;
        }
    }


}

