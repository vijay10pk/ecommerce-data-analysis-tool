using System;
using Microsoft.AspNetCore.Mvc;
using EcommerceDataAnalysisToolServer.Models;
using EcommerceDataAnalysisToolServer.Repository;
using EcommerceDataAnalysisToolServer.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace EcommerceDataAnalysisToolServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesDataAnalysisController : ControllerBase
    {
        //Create an object 
        private readonly ILogger<SalesDataAnalysisController> _logger;
        private readonly ISalesDataAnalysisRepository _salesDataAnalysisRepository;

        /// <summary>
        /// Constructor to initialize objects
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="expenseAnalysisRepository"></param>
        public SalesDataAnalysisController(ILogger<SalesDataAnalysisController> logger, ISalesDataAnalysisRepository salesDataAnalysisRepository)
        {
            _logger = logger;
            _salesDataAnalysisRepository = salesDataAnalysisRepository;
        }

        /// <summary>
        /// SalesDataAnalysis - Gets all data from the file
        /// </summary>
        /// <returns>all fetched sales data</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Ecommerce>))]
        public IActionResult GetAllSales()
        {
            return Ok(_salesDataAnalysisRepository.GetAllSales());
        }

        /// <summary>
        /// SalesDataAnalysis/{id} - end point for getting sales data for the given id
        /// </summary>
        /// <param name="id">sales data id</param>
        /// <returns>fetched sales data of the given id</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Ecommerce))]
        [ProducesResponseType(404)]
        public IActionResult GetSalesById(int id)
        {
            Ecommerce sales = _salesDataAnalysisRepository.GetSalesById(id);
            if (sales == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(sales);
            }

        }


        /// <summary>
        /// SalesDataAnalysis/search/{productName} - end point for searching sales data for the given product name
        /// </summary>
        /// <param name="productName">sales data by productName</param>
        /// <returns>search sales data for the given productName</returns>
        [HttpGet("{name}")]
        [ProducesResponseType(200, Type = typeof(Ecommerce))]
        [ProducesResponseType(404)]
        public IActionResult GetSalesByName(string name)
        {
            Ecommerce sales = _salesDataAnalysisRepository.SearchSalesByProductName(name);
            if (sales == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(sales);
            }


        }
        /// <summary>
        /// SalesDataAnalysis - end point for Create/Add new sales data
        /// </summary>
        /// <param name="expense">new sales data</param>
        /// <returns>success or failure response</returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AddSalesData([FromBody] Ecommerce saleData)
        {
            if (saleData == null)
            {
                return BadRequest("Sales is null");
            }
            bool result = _salesDataAnalysisRepository.AddSalesData(saleData);
            if (result)
            {
                return Ok("Successfully added");
            }
            else
            {
                return BadRequest("Sales data not added");
            }

        }

        /// <summary>
        /// SalesDataAnalysis/{id} - end point for updating the data for the given sales data id
        /// </summary>
        /// <param name="expense">updated sales data</param>
        /// <returns>success or failure response</returns>
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult EditSalesData([FromBody] Ecommerce saleData)
        {
            if (saleData == null)
            {
                return BadRequest("sale data is null");
            }
            bool isUpdated = _salesDataAnalysisRepository.EditSalesData(saleData);
            if (!isUpdated)
            {
                return NotFound("No matching sale data");
            }
            else
            {
                return Ok("Successfully updated");
            }
        }

        /// <summary>
        /// SalesDataAnalysis/{id} -  End point for deleting the data based on sales data id
        /// </summary>
        /// <param name="id">sales data id</param>
        /// <returns>success or failure response</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteSalesData([FromRoute] int id)
        {
            bool deleted = _salesDataAnalysisRepository.DeleteSalesData(id);

            if (!deleted)
            {
                return NotFound("No matching id");
            }
            else
            {
                return Ok("sale data deleted");
            }
        }

        /// <summary>
        /// Method to get the total revenue for the given year
        /// </summary>
        /// <param name="year">Year for which we want to get total revenue</param>
        /// <returns>Total revenue of the year</returns>
        [HttpGet("total-revenue/{year}")]
        public IActionResult GetTotalRevenueForYear(int year)
        {
            double totalRevenueInYear = _salesDataAnalysisRepository.GetTotalRevenueForYear(year);
            return Ok(totalRevenueInYear);
        }
        /// <summary>
        /// Method to get the name of category which has highest sales in a year or month
        ///month is optional
        /// </summary>
        /// <param name="year">Year for which we want to get the highest sales</param>
        /// <returns>name of the category</returns>
        [HttpGet("salesByCategory/{year}")]
        public IActionResult GetHighestSalesByCategory(int year, int month)
        {
            CategoryData category = _salesDataAnalysisRepository.GetCategoryWhichHasHighestSales(year, month);
            return Ok(category);
        }

        /// <summary>
        /// GET the most sold product in a year
        /// </summary>
        /// <param name="year">Year</param>
        /// <returns>highest sold product in that year</returns>
        [HttpGet("highestsoldproduct/{year}")]
        public async Task<ActionResult<Ecommerce>> GetHighestSoldProductForYear(int year)
        {
            var result = await _salesDataAnalysisRepository.GetHighestSoldProductForYear(year);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// GET most sold product in a selected month for given year
        /// </summary>
        /// <param name="month">Month in MM format</param>
        /// <param name="year">Year in YYYY format</param>
        /// <returns>most sold product in the selected month</returns>
        [HttpGet("highest-sold-product-in-month/{month}/{year}")]
        public async Task<ActionResult<string>> GetHighestSoldProductForMonthAndYear(int month, int year)
        {
            var sales = await _salesDataAnalysisRepository.GetSalesForMonthAndYearAsync(month, year);
            var highestSoldProduct = sales.GroupBy(s => s.ProductName)
                                          .OrderByDescending(g => g.Count())
                                          .Select(g => g.Key)
                                          .FirstOrDefault();

            if (highestSoldProduct == null)
            {
                return NotFound("This month doesn't have highest sold product");
            }

            return Ok(highestSoldProduct);
        }

        /// <summary>
        /// Method to get the average sales in a year
        /// </summary>
        /// <param name="year">Year for which we want to get average sale</param>
        /// <returns>Average sales of the year</returns>
        [HttpGet("averageSales/{year}")]
        public IActionResult GetAverageSalesForYear(int year)
        {
            double totalSalesInYear = _salesDataAnalysisRepository.GetTotalRevenueForYear(year);
            double getTotalSales = _salesDataAnalysisRepository.GetTotalCountSales(year);
            double averageSaleInYear = System.Math.Truncate(totalSalesInYear / 12);
            return Ok(averageSaleInYear);
        }

        /// <summary>
        /// Method to get the average sales in a month
        /// </summary>
        /// <param name="month">Month in MM format</param>
        /// <param name="year">Year in YYYY format</param>
        /// <returns>Average sales of the month in a given year</returns>
        [HttpGet("averageSales/{year}/{month}")]
        public IActionResult GetAverageSalesInAMonth(int month, int year)
        {
            var salesInMonth = _salesDataAnalysisRepository.GetAverageSalesForMonth(month, year);
            var salesInYear = _salesDataAnalysisRepository.GetTotalSalesRecordInMonthForYear(month, year);
            var averageSalesInMonth = System.Math.Truncate(salesInMonth / 30);

            return Ok(averageSalesInMonth);
        }

        /// <summary>
        /// Method to get the data based on year
        ///
        /// </summary>
        /// <param name="year">Year for which we want to get the highest sales,
        /// highest sold product, total revenue</param>
        /// <returns>sales summary</returns>
        [HttpGet("DataBasedOnYear/{year}")]
        public async Task<ActionResult<SalesSummary>> GetFilterBaseOnYear(int year)
        {
            SalesSummary salesSummary = await _salesDataAnalysisRepository.GetFilterBaseOnYear(year);
            return Ok(salesSummary);
        }

        /// <summary>
        /// GetPredictionForRevenue
        /// </summary>
        /// <returns></returns>
        [HttpGet("predictionOnRevenue")]
        public IActionResult GetPredictionForRevenue()
        {
            CategoryData data = _salesDataAnalysisRepository.GetPredictionForRevenue();
            return Ok(data);
        }

        /// <summary>
        /// GetPredictionForCategory
        /// </summary>
        /// <returns></returns>
        [HttpGet("predictionOnCategory")]
        public IActionResult GetPredictionForCategory()
        {
            CategoryData data = _salesDataAnalysisRepository.GetPredictionForCategory();
            return Ok(data);
        }

        /// <summary>
        /// GetPredictionForCategoryOnMonth
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet("predictionOnCategoryOnMonth")]
        public IActionResult GetPredictionForCategoryOnMonth(int year)
        {
            CategoryData data = _salesDataAnalysisRepository.GetPredictionForCategoryOnMonth(year);
            return Ok(data);
        }

        /// <summary>
        /// GET sales forecast like forecasted revenue, what category to sell and what product to sell
        /// </summary>
        /// <returns>Sales Forecast</returns>
        [HttpGet("prediction")]
        public IActionResult GetPredictionData()
        {
            string data = _salesDataAnalysisRepository.GetPredictionData();
            return Ok(data);
        }


    }
}
