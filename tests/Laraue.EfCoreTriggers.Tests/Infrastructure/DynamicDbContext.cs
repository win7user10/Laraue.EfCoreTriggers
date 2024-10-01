using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Laraue.EfCoreTriggers.Tests.Infrastructure
{
    public class DynamicDbContext : DbContext
    {
        private readonly Action<ModelBuilder> _setupDbContext;

        public DbSet<SourceEntity> SourceEntities { get; set; }
        public DbSet<DestinationEntity> DestinationEntities { get; set; }
        public DbSet<RelatedEntity> RelatedEntities { get; set; }

        public DynamicDbContext(DbContextOptions<DynamicDbContext> options, Action<ModelBuilder> setupDbContext = null)
            : base(options)
        {
            _setupDbContext = setupDbContext;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DestinationEntity>()
                .HasIndex(x => x.UniqueIdentifier)
                .IsUnique();
            
            _setupDbContext?.Invoke(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        public override async ValueTask DisposeAsync()
        {
            await Database.EnsureDeletedAsync();
            await base.DisposeAsync();
        }

        public override void Dispose()
        {
            var relationalModel = GetRelationalModel();

            var migrationsModelDiffer = Database.GetService<IMigrationsModelDiffer>();
            var downMigrationDifferences = migrationsModelDiffer
                .GetDifferences(relationalModel, null);

            var migrationsSqlGenerator = Database.GetService<IMigrationsSqlGenerator>();
            var migrationCommands = migrationsSqlGenerator
                .Generate(downMigrationDifferences);
            
            foreach (var command in migrationCommands)
            {
                Database.ExecuteSqlRaw(command.CommandText);
            }
            
            Database.EnsureDeleted();
            base.Dispose();
        }

        private IRelationalModel GetRelationalModel()
        {
#if NET5_0
            return Model.GetRelationalModel();
#else
            return Database.GetService<IDesignTimeModel>().Model.GetRelationalModel();
#endif
        }
    }
}