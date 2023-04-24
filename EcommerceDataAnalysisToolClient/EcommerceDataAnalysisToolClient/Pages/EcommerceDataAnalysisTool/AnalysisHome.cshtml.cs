using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using EcommerceDataAnalysisToolServer.Models;

namespace EcommerceDataAnalysisToolClient.Pages.EcommerceDataAnalysisTool
{
    /// <summary>
    /// calling Filter  dataAPI
    /// </summary>
	public class AnalysisHomeModel : PageModel
    {

        //Getter and setter for year and total revenue
        public SalesSummary? Analysis = new();
        [BindProperty(SupportsGet = true)]
        public int? Year { get; set; }


        /// <summary>
        /// on get call to API
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (Year.HasValue)
            {
                try
                {
                    //GET Data Analysis for the selected year
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://localhost:7266");
                        var response = await client.GetAsync($"https://localhost:7267/SalesDataAnalysis/DataBasedOnYear/{Year}");
                        response.EnsureSuccessStatusCode();
                        var responseContent = await response.Content.ReadAsStringAsync();
                        Analysis = JsonConvert.DeserializeObject<SalesSummary>(responseContent);
                        Console.WriteLine(Analysis);
                    }
                }
                //Handles exception
                catch (HttpRequestException)
                {
                    ModelState.AddModelError(string.Empty, $"Error getting Data Analysis for {Year}");
                }
            }
            return Page();
        }

    }

}


