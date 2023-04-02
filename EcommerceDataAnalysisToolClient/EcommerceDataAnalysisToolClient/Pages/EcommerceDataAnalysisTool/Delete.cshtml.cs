using System;
using System.Text.Json;
using EcommerceDataAnalysisToolServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace EcommerceDataAnalysisToolClient.Pages.EcommerceDataAnalysisTool
{
    /// <summary>
    /// Class that handles delete functionality
    /// </summary>
    public class DeleteModel : PageModel
    {
        public Ecommerce sales = new();
        public string errorMessage = "";
        public string successMessage = "";
        /// <summary>
        /// Get the the sales data of the chosen id
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
        /// Delete the selected sales data
        /// </summary>
        public async Task<IActionResult> OnPost()
        {
            int id = int.Parse(Request.Form["id"]);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:7266");
                var result = await client.DeleteAsync("https://localhost:7267/SalesDataAnalysis/" + id);
                string resultContent = await result.Content.ReadAsStringAsync();
                if (!result.IsSuccessStatusCode)
                {
                    errorMessage = "Error while deleting";
                }
                else
                {
                    successMessage = "Successfully deleted";
                    
                }
                RedirectToPage("./index");
            }
            return Page();
        }
    }
}

