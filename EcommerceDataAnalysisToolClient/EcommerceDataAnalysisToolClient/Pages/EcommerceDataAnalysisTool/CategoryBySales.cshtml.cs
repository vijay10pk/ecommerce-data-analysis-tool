using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace EcommerceDataAnalysisToolClient.Pages.EcommerceDataAnalysisTool
{
    public class CategoryBySalesModel : PageModel
    {
        public string category = "";
        public CategoryData cd = new CategoryData();
        public string season = "";
        //adding below values to show the user given values in web page
        public string givenYear = "";
        public string givenmonth = "";
        public CategoryData CategoryWithHighestSales { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string year = Request.Query["year"];
            string month = Request.Query["month"];
            //assigning values with the user request
            givenYear = year;
            givenmonth = month;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7266");
                var response = await client.GetAsync($"https://localhost:7267/SalesDataAnalysis/salesByCategory/{year}");
                if (month != null && month != "")
                {
                    response = await client.GetAsync($"https://localhost:7267/SalesDataAnalysis/salesByCategory/{year}?month={month}");
                }
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    CategoryWithHighestSales = JsonConvert.DeserializeObject<CategoryData>(content);
                }
                else
                {
                    CategoryWithHighestSales = new CategoryData();
                   // CategoryWithHighestSales.ErrorMessage = $"Error: {response.StatusCode}";

                }
            }
            return Page();
        }
    }
}
