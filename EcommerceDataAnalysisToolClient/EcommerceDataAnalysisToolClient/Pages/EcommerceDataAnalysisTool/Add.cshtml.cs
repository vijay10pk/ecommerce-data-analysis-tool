using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EcommerceDataAnalysisToolServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

/* adding and saving data page */
namespace EcommerceDataAnalysisToolClient.Pages.EcommerceDataAnalysisTool
{
    public class AddModel : PageModel
    {
        public Ecommerce ecommerce = new();
        public string errorMessage = "";
        public string successMessage = "";

        public async void OnPost()
        {       
            ecommerce.ProductName = Request.Form["productName"];
            ecommerce.ProductCategory = Request.Form["productCategory"];
            ecommerce.Price = int.Parse(Request.Form["price"]);
            ecommerce.Brand = Request.Form["brand"];

           
            
                var opt = new JsonSerializerOptions() { WriteIndented = true };
                string json = System.Text.Json.JsonSerializer.Serialize<Ecommerce>(ecommerce, opt);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7267/");
                    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    var result = await client.PostAsync("/SalesDataAnalysis", content);
                    string resultContent = await result.Content.ReadAsStringAsync();
                    Console.WriteLine(resultContent);
              

                    if (!result.IsSuccessStatusCode)
                    {
                        errorMessage = "Error adding";
               
                    }
                    else
                    {
                        successMessage = "Successfully added";
                    }
                }
            
        }
    }
}
