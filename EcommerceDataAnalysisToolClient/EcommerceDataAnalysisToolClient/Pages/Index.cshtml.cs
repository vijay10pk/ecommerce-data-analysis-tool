using EcommerceDataAnalysisToolServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace EcommerceDataAnalysisToolClient.Pages.EcommerceDataAnalysisTool;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public List<Ecommerce> Ecommerce = new();

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public async void OnGet()
    {
        //connection to call endpoint
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://localhost:7266");
            //HTTP GET
            var responseTask = client.GetAsync("https://localhost:7267/SalesDataAnalysis");
            responseTask.Wait();

            //declaration of result
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = await result.Content.ReadAsStringAsync();
                Ecommerce = JsonConvert.DeserializeObject<List<Ecommerce>>(readTask);
            }
        }
    }
}

