namespace EcommerceDataAnalysisToolServer.Models
{
            public class CategoryData
        {
            public CategoryData() { }

            public CategoryData(string Category, string Season, string PredictedCategory, double PredictedRevenue)
            {
                Category = category;
                Season = season;
            PredictedCategory = predictedCategory;
             PredictedRevenue = predictedRevenue;
                
            }
            public string category { get; set; }
            public string? season { get; set; }

        public string? predictedCategory { get; set; }
        public double predictedRevenue { get; set; }

    }
    }
