using System;
using EcommerceDataAnalysisToolServer.Models;
namespace EcommerceDataAnalysisToolServer.Interfaces
{
    /// <summary>
    /// Method definition for all the methods in repository
    /// </summary>
	public interface ISalesDataAnalysisRepository
	{
        ICollection<Ecommerce> GetAllSales();
        Ecommerce GetSalesById(int id);
        bool SalesDataExists(int id);
        bool AddSalesData(Ecommerce saleData);
        bool EditSalesData(Ecommerce updateSaleData);
        bool DeleteSalesData(int id);
        double GetTotalRevenueForYear(int year);
        CategoryData GetCategoryWhichHasHighestSales(int year, int month);
        Task<Ecommerce> GetHighestSoldProductForYear(int year);
        Task<List<Ecommerce>> GetSalesForMonthAndYearAsync(int month, int year);
        double GetTotalSales();
        double GetAverageSalesForMonth(int month, int year);

        string GetFilterBaseOnYear(int year);

        CategoryData GetPredictionForCategory();

        CategoryData GetPredictionForCategoryOnMonth(int year);

        CategoryData GetPredictionForRevenue();

        IQueryable<Ecommerce> SearchSalesByProductName(String productName);
    }

}

