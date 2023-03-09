using System;
using System.Globalization;
using EcommerceDataAnalysisToolServer.Data;
using EcommerceDataAnalysisToolServer.Models;

namespace EcommerceDataAnalysisToolServer
{
	public class Seed
	{
		private readonly DataContext dataContext;
        List<Ecommerce> ecommerces;

        public Seed(DataContext dataContext)
		{
			this.dataContext = dataContext;
		}

        /// <summary>
        /// This method get the data from the ReadDataFromCsv method and store it in the database
        /// </summary>
		public void SeedDataContext()
		{
			if (!dataContext.Ecommerce.Any())
			{

                ecommerces = new();
                ReadDataFromCsv();

                dataContext.Ecommerce.AddRange(ecommerces);
                dataContext.SaveChanges();
            }
		}

        /// <summary>
        /// Method that read data from the csv file.
        /// </summary>
        private void ReadDataFromCsv()
        {
            try
            {
                using (StreamReader sr = new StreamReader("./Data/ecommerce.csv"))
                {
                    int c = 0;
                    while (!sr.EndOfStream)
                    {
                        string[] line = sr.ReadLine().Split(',');
                        if (c != 0)
                        {
                            DateTime d = DateTime.ParseExact(line[2], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            int i = 5;
                            while (line[i].ToLower().Contains('>') || line[i].ToLower().Contains(']') || line[i].ToLower().Contains('['))
                            {
                                line[4] += "," + line[i];
                                i += 1;
                            }
                            double p1 = Convert.ToDouble(line[i]);
                            double p2 = Convert.ToDouble(line[i+1]);
                            //string[] line = sr.ReadLine().Split(',');
                            //   DateTime.ParseExact(s, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                            ecommerces.Add(new Ecommerce(0,d,
                                line[3], line[4],p1,p2, line[i+2]));
                        }
                        c += 1;
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
            //return bills;

            }

    }
}

