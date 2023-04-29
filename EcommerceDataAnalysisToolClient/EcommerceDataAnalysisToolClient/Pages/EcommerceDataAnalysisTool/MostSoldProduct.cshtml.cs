using System;
using EcommerceDataAnalysisToolServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Globalization;

namespace EcommerceDataAnalysisToolClient.Pages.EcommerceDataAnalysisTool
{
	public class MostSoldProductModel: PageModel
	{
        //Getter and setter for year and total revenue
        public Ecommerce Product = new();
        public string? ProductMonth { get; private set; }
        public string? DisplayBy { get; private set; }
        [BindProperty(SupportsGet = true)]
        public int? Year { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? Month { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Year.HasValue && Month == null)
            {
                DisplayBy = "year";
                try
                {
                    //GET total revenue for the selected year
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://localhost:7266");

                        var response = await client.GetAsync($"https://localhost:7267/SalesDataAnalysis/highestsoldproduct/{Year}");
                        response.EnsureSuccessStatusCode();
                        if (response.IsSuccessStatusCode)
                        {
                            var responseTask = await response.Content.ReadAsStringAsync();
                            Product = JsonConvert.DeserializeObject<Ecommerce>(responseTask);
                        }
                    }
                }
                //Handles exception
                catch (HttpRequestException)
                {
                    ModelState.AddModelError(string.Empty, $"Error getting most sold product for {Year}");
                }
            }
            else if (Year.HasValue && Month.HasValue)
            {
                DisplayBy = "month";
                try
                {
                    //GET total revenue for the selected month and year
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://localhost:7266");
                        var response = await client.GetAsync($"https://localhost:7267/SalesDataAnalysis/highest-sold-product-in-month/{Month}/{Year}");
                        response.EnsureSuccessStatusCode();
                        if (response.IsSuccessStatusCode)
                        {
                            ProductMonth = await response.Content.ReadAsStringAsync();
                            Console.WriteLine(ProductMonth);
                        }
                    }
                }
                //Handles exception
                catch (HttpRequestException)
                {
                    ModelState.AddModelError(string.Empty, $"Error getting most sold product for {Month}");
                }
            }
            return Page();
        }
    }
}

