using System;
namespace EcommerceDataAnalysisToolServer.Models
{
	public class SalesSummary
	{
        public SalesSummary()
        {
        }

        public SalesSummary( double totalRevenue, CategoryData category, string averageSales)
        {
            TotalRevenue = totalRevenue;
            CategoryWithHighestSales = category;
            AverageSaleInYear = averageSales;

        }
        public double TotalRevenue { get; set; }
        public CategoryData CategoryWithHighestSales { get; set; }
        public string AverageSaleInYear { get; set; }
    }
}

