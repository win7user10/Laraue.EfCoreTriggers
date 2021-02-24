using Microsoft.EntityFrameworkCore;

namespace Laraue.EfCoreTriggers.SqlServerTests.Issues._3
{
	public class TestDbContext : DbContext
	{
		public TestDbContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<SalesCategory> SalesCategories { get; set; }
	}
}