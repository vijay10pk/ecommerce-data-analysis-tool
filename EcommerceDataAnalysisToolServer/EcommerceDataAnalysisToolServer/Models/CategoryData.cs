namespace EcommerceDataAnalysisToolServer.Models
{
            public class CategoryData
        {
            public CategoryData() { }

            public CategoryData(string Category, string Season)
            {
                Category = category;
                Season = season;
            }
            public string category { get; set; }
            public string? season { get; set; }
        }
    }
