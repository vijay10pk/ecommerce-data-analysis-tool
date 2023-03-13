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
        string GetCategoryWhichHasHighestSales(int year, int month);
        string GetFilterBaseOnYear(int year);
    }

}

