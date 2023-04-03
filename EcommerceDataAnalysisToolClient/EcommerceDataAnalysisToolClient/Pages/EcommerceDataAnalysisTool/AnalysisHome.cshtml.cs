using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceDataAnalysisToolClient.Pages.EcommerceDataAnalysisTool
{
    /// <summary>
    /// calling Filter  dataAPI
    /// </summary>
	public class AnalysisHomeModel : PageModel
    {
        public string category = "";
        public CategoryData cd = new CategoryData();
        //added variable to show user requested year in the web page
        public string givenYear = "";
        /// <summary>
        /// on get call to API
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGet()
        {
            string year = Request.Query["year"];
            //assigning user sent value to variable
            givenYear = year;
            if (year == null || year == "") { year = "2015"; }

            using (var client = new HttpClient())
            {
                //making connection
                client.BaseAddress = new Uri("https://localhost:7266");
                var response = await client.GetAsync($"https://localhost:7267/SalesDataAnalysis/DataBasedOnYear/{year}");

                if (response.IsSuccessStatusCode)
                {
                    category = await response.Content.ReadAsStringAsync();
                    category = category.Substring(9);
                    Console.WriteLine(category);
                }
                else
                {
                    category = $"Error: {response.StatusCode}";
                }
            }
            return Page();
        }

    }

}


