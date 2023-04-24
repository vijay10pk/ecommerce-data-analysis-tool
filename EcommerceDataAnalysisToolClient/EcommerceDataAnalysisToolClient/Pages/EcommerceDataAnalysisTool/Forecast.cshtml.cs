using System;
using EcommerceDataAnalysisToolServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EcommerceDataAnalysisToolClient.Pages.EcommerceDataAnalysisTool
{
    public class ForecastModel : PageModel
    {

        //Getter and setter for year and total revenue
        public JObject Prediction = new();

        /// <summary>
        /// Get sales Forecast for the upcoming year
        /// </summary>
        public async void OnGet()
        {
            //connection to call Cereal endpoint
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:7266");
                //HTTP GET
                var responseTask = client.GetAsync("https://localhost:7267/SalesDataAnalysis/prediction");
                responseTask.Wait();

                //declaration of result
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();
                    Prediction = JsonConvert.DeserializeObject<JObject>(readTask);
                }
            }
        }
    }
}

