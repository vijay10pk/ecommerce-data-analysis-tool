using System;
using Microsoft.EntityFrameworkCore;
using EcommerceDataAnalysisToolServer.Models;
namespace EcommerceDataAnalysisToolServer.Data
{
	/// <summary>
	/// Initialize new instance with database
	/// </summary>
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{
		}
		public DbSet<Ecommerce> Ecommerce { get; set; }
	}
}

