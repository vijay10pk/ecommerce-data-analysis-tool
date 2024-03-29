﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

//This class handles API call for getting total revenue for the given year
namespace EcommerceDataAnalysisToolClient.Pages.EcommerceDataAnalysisTool
{
    public class TotalRevenueModel : PageModel
    {

        //Getter and setter for year and total revenue
        public decimal? Revenue { get; private set; }
        [BindProperty(SupportsGet = true)]
        public int? Year { get; set; }

        /// <summary>
        /// Get Total revenue for that particular year
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            if (Year.HasValue)
            {
                try
                {
                    //GET total revenue for the selected year
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://localhost:7266");
                        var response = await client.GetAsync($"https://localhost:7267/SalesDataAnalysis/total-revenue/{Year}");
                        response.EnsureSuccessStatusCode();
                        var responseContent = await response.Content.ReadAsStringAsync();
                        Revenue = decimal.Parse(responseContent);
                    }
                }
                //Handles exception
                catch (HttpRequestException)
                {
                    ModelState.AddModelError(string.Empty, $"Error getting total revenue for {Year}");
                }
            }
            return Page();
        }
    }
}
