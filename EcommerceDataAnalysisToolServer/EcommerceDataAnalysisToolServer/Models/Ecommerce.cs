using System;
namespace EcommerceDataAnalysisToolServer.Models
{
    public class Ecommerce
    {
        public Ecommerce()
        {
        }

        /// <summary>
        /// Constructor for bill object
        /// </summary>
        /// <param name="date">date of the bill</param>
        /// <param name="amount">Bill Amount</param>
        /// <param name="category">Category of expense</param>
        public Ecommerce(int sno, DateTime crawlTimestamp, string productName, string productCategoryTree, double retailPrice, double discountedPrice, string brand)
        {
            Sno = sno;
            CrawlTimestamp = crawlTimestamp;
            ProductName = productName;
            ProductCategoryTree = productCategoryTree;
            RetailPrice = retailPrice;
            DiscountedPrice = discountedPrice;
            Brand = brand;

        }
        //Getting and setting the value with with the variables.
        public int Id { get; set; }
        public int Sno { get; set; }
        public DateTime CrawlTimestamp { get; set; }
        public string ProductName { get; set; }
        public string ProductCategoryTree { get; set; }
        public double RetailPrice { get; set; }
        public double DiscountedPrice { get; set; }
        public string Brand { get; set; }
    }
}

