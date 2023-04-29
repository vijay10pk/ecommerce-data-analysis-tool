using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using EcommerceDataAnalysisToolServer.Models;

namespace EcommerceDataAnalysisToolClient.Pages.EcommerceDataAnalysisTool
{
    public class EditSalesDataModel : PageModel
    {
        public Ecommerce sales = new();
        public string errorMessage = "";
        public string successMessage = "";

        /// <summary>
        /// Get the sales data by ID for the selected sales
        /// </summary>
        public async void OnGet()
        {
            string id = Request.Query["id"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:7266");
                //HTTP  GET
                var responseTask = client.GetAsync("https://localhost:7267/SalesDataAnalysis/" + id);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();
                    sales = JsonConvert.DeserializeObject<Ecommerce>(readTask);
                }
            }
        }


        /// <summary>
        /// Update the sales of the selected ID
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            sales.Id = int.Parse(Request.Form["id"]);
            sales.Date = DateTime.Parse(Request.Form["date"]);
            sales.ProductName = Request.Form["productName"];
            sales.ProductCategory = Request.Form["productCategory"];
            sales.Price = double.Parse(Request.Form["price"]);
            sales.Brand = Request.Form["brand"];

            if (sales.Price == null && sales.ProductName.Length == 0 && sales.ProductCategory.Length == 0 && sales.Brand.Length == 0)
            {
                errorMessage = "Enter all the required fields";
            }
            else
            {
                var opt = new JsonSerializerOptions() { WriteIndented = true };
                string json = System.Text.Json.JsonSerializer.Serialize<Ecommerce>(sales, opt);
                using (var client = new HttpClient())
                {
                    //HTTP PUT - update the particular sale data
                    client.BaseAddress = new Uri("http://localhost:7266");
                    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    var result = await client.PutAsync("https://localhost:7267/SalesDataAnalysis/", content);
                    string resultContent = await result.Content.ReadAsStringAsync();
                    Console.WriteLine(resultContent);
                    //Handle Exception
                    if (!result.IsSuccessStatusCode)
                    {
                        errorMessage = "Error editing";
                    }
                    else
                    {
                        successMessage = "Successfully edited";
                        Console.WriteLine("successMessage: " + successMessage);
                    }
                }
            }
            return Page();
        }

    }
}
