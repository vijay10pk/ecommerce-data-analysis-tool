using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceDataAnalysisToolClient.Pages.EcommerceDataAnalysisTool
{
    public class TotalRevenueModel : PageModel
    {
        //private readonly HttpClient _httpClient;

        //public TotalRevenueModel(HttpClient httpClient)
        //{
        //    _httpClient = httpClient;
        //}

        public decimal? Revenue { get; private set; }

        [BindProperty(SupportsGet = true)]
        public int? Year { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Year.HasValue)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://localhost:7266");
                        var response = await client.GetAsync($"https://localhost:7267/SalesDataAnalysis/total-revenue/{Year}");
                        response.EnsureSuccessStatusCode();
                        var responseContent = await response.Content.ReadAsStringAsync();
                        Revenue = decimal.Parse(responseContent);
                    }
                }
                catch (HttpRequestException)
                {
                    ModelState.AddModelError(string.Empty, $"Error getting total revenue for {Year}");
                }
            }
            return Page();
        }
    }
}
