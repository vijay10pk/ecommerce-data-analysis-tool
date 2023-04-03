using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace EcommerceDataAnalysisToolClient.Pages.EcommerceDataAnalysisTool
{
    /// <summary>
    /// calling salesByCategory API
    /// </summary>
    public class CategoryBySalesModel : PageModel
    {

        public string category = "";
        public CategoryData cd = new CategoryData();
        /// <summary>
        /// calling get API
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGet()
        {
            string year = Request.Query["year"];
            string month = Request.Query["month"];

            using (var client = new HttpClient())
            {
                //connection to API
                client.BaseAddress = new Uri("https://localhost:7266");
                var response = await client.GetAsync($"https://localhost:7267/SalesDataAnalysis/salesByCategory/{year}");
                if (month != null && month!="")
                {
                     response = await client.GetAsync($"https://localhost:7267/SalesDataAnalysis/salesByCategory/{year}?{month}="+month);
                }
              
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
