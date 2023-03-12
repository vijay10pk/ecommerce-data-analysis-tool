using System;
using Microsoft.AspNetCore.Mvc;
using EcommerceDataAnalysisToolServer.Models;
using EcommerceDataAnalysisToolServer.Repository;
using EcommerceDataAnalysisToolServer.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public IActionResult DeleteSalesData(int id)
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

    }
}

