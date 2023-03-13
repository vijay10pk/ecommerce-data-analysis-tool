using System;
using EcommerceDataAnalysisToolServer.Models;
using EcommerceDataAnalysisToolServer.Data;
using EcommerceDataAnalysisToolServer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

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
            updatedSaleData.Date = DateTime.UtcNow;
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

            return totalSales;
        }

        /// <summary>
        /// Method to get the category which has the highest sales in a year or month
        /// </summary>
        /// <param name="year">Year</param>
        /// <returns>name of the category</returns>
        public string GetCategoryWhichHasHighestSales(int year, int month)
        {
            var startDate = new DateTime();
            var endDate = new DateTime();
            var season = "";
            String seasonAndCategory = "";

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
            if (season != "")
                seasonAndCategory = "season has highest sale in the year:    " + season + "\n" + "category:  " + category.ProductCategory + "";
            else
                seasonAndCategory = "category:  " + category.ProductCategory + "";
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
        /// Method to get the sales in the particular month of the given year
        /// </summary>
        /// <param name="month">month in MM format</param>
        /// <param name="year">year in MM format</param>
        /// <returns>sales from the particular month of the given year</returns>
        public double GetAverageSalesForMonth(int month, int year)
        {
            var saleInMonth = _context.Ecommerce
                                    .Where(s => s.Date.Year == year && s.Date.Month == month)
                                    .Sum(s => s.Price);
            return saleInMonth;
        }

        public string GetFilterBaseOnYear(int year)
        {
            double TotalRevenue = GetTotalRevenueForYear(year);
            String GetCategory = GetCategoryWhichHasHighestSales(year, 0);
            //add code for average sales
            double totalSalesInYear = GetTotalRevenueForYear(year);
            double getTotalSales = GetTotalSales();
            double averageSaleInYear = (totalSalesInYear / getTotalSales);
            //add code for highest sold product
           // Task<Ecommerce> highestSaledProduct = GetHighestSoldProductForYear(year);
            var data = "TotalRevenue:    " + TotalRevenue + "\n" + "category which sold most:  " + GetCategory + "\n" + "average sale in the year: " + averageSaleInYear + "";

            return data;
        }
    }
}

