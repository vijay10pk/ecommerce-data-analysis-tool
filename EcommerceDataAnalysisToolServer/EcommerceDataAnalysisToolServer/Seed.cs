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
                using (StreamReader sr = new StreamReader("./Data/new-ecommerce.csv"))
                {
                    int c = 0;
                    while (!sr.EndOfStream)
                    {
                        string[] line = sr.ReadLine().Split(',');
                        if (c != 0 && line.Length >= 4)
                        {
                            DateTime d = DateTime.Parse(line[0]);

                            string category = "";
                            if (line[2].Contains("[\"\""))
                            {
                                int startIndex = line[2].IndexOf("[\"\"") + 3;
                                int endIndex = line[2].IndexOf(">");
                                if (startIndex >= 0 && endIndex >= 0)
                                {
                                    category = line[2].Substring(startIndex, endIndex - startIndex).Trim();
                                }
                            }

                            double p1 = 0;
                            if (double.TryParse(line[3], out p1))
                            {
                                ecommerces.Add(new Ecommerce(d, line[1], category, p1, line[5]));
                            }
                        }
                        c += 1;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
        }


    }
}

