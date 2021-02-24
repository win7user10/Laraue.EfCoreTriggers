using Microsoft.EntityFrameworkCore;

namespace Laraue.EfCoreTriggers.SqlServerTests.Issues._3
{
	public class TestDbContext : DbContext
	{
		public TestDbContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<SalesCategory> SalesCategories { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<DomainBase>()
				.ToTable("DomainEntityBase", "CommonSchema")
				.HasKey(f => f.Id);

			modelBuilder.Entity<SalesCategory>()
				.ToTable("SalesCategory", "PolicySchema");

			modelBuilder.Entity<SalesArea>()
				.ToTable("SalesArea");

        }
    }
}