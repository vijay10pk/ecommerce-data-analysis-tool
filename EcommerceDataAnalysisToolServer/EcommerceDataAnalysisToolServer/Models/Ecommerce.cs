using System;
namespace EcommerceDataAnalysisToolServer.Models
{
    public class Ecommerce
    {
        public Ecommerce()
        {
        }

        public Ecommerce(DateTime date, string productName, string productCategory, double retailPrice, string brand)
        {
            Date = date;
            ProductName = productName;
            ProductCategory = productCategory;
            Price = retailPrice;
            Brand = brand;

        }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public double Price { get; set; }
        public string Brand { get; set; }
    }
}

