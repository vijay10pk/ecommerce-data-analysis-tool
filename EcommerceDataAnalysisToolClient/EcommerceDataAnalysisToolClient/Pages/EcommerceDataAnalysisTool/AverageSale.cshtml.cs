using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceDataAnalysisToolClient.Pages.EcommerceDataAnalysisTool
{
    public class AverageSaleModel : PageModel
    {
       

        public decimal? Average { get; private set; }

        [BindProperty(SupportsGet = true)]
        public int? Year { get; set; }


        /// <summary>
        /// Get the average sales of the particular year
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            
            if (Year.HasValue)

            {
                try
                {
                    using (var client = new HttpClient())
                    {

                        //HTTP GET
                        client.BaseAddress = new Uri("http://localhost:7266");
                        var response = await client.GetAsync($"https://localhost:7267/SalesDataAnalysis/averageSales/{Year}");
                        response.EnsureSuccessStatusCode();
                        var responseContent = await response.Content.ReadAsStringAsync();
                        Average = decimal.Parse(responseContent);
                    }
                }
                catch (HttpRequestException)
                {
                    ModelState.AddModelError(string.Empty, $"Error getting average sales for {Year}");
                }
            }
            return Page();
        }
    }
}
