using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.PostgreSql.Extensions;
using Laraue.Linq2Triggers.Core;
using Microsoft.EntityFrameworkCore;

namespace Laraue.EfCoreTriggers.TestMigration;

public class TestDbContext : DbContext
{
    public DbSet<Entity1> Users { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "server=localhost;user=postgres;password=postgres;database=efcoretriggers;",
                x => x
                    .MigrationsAssembly(typeof(TestDbContext).Assembly.FullName))
            .UsePostgreSqlTriggers();
        
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Constants.AnnotationKey = "lc_trigger_";
        Constants.GetTriggerName = (
            triggerTime,
            triggerEvent,
            triggerEntityType) => $"{triggerTime}_{triggerEvent}_{triggerEntityType.Name}".ToLower();
        
        modelBuilder.Entity<Entity1>()
            .AfterDelete(x => x
                .Action(y => y
                    .Insert(inserted => new Entity1 { Id = inserted.Old.Id + 1 })));
        
        modelBuilder.Entity<Entity2>()
            .AfterDelete(x => x
                .Action(x => x
                    .Insert(inserted => new Entity1 { Id = inserted.Old.Id })));
        
        base.OnModelCreating(modelBuilder);
    }
}