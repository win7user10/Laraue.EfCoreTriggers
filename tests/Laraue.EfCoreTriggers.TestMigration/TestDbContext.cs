using System;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.MySql;
using Laraue.EfCoreTriggers.MySql.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Laraue.EfCoreTriggers.TestMigration;

public class TestDbContext : DbContext
{
    public DbSet<Entity1> Entities1 { get; set; }
    
    public DbSet<Entity2> Entities2 { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(
            "server=localhost;user=mysql;password=mysql;database=efcoretriggers;", 
            new MySqlServerVersion(new Version(8, 0, 22)),
                x => x
                    .MigrationsAssembly(typeof(TestDbContext).Assembly.FullName))
            .UseMySqlTriggers();
        
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entity1>()
            .AfterDelete(x => x
                .Action(x => x
                    .Insert(inserted => new Entity1 { Id = inserted.Id })));
        
        base.OnModelCreating(modelBuilder);
    }
}