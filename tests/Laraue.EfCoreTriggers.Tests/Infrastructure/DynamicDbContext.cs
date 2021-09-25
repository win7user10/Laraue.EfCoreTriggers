using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Tests.Tests;
using Microsoft.EntityFrameworkCore;

namespace Laraue.EfCoreTriggers.Tests.Infrastructure
{
    public class DynamicDbContext : DbContext
    {
        private readonly Action<ModelBuilder> _setupDbContext;

        public DbSet<SourceEntity> SourceEntities { get; set; }
        public DbSet<DestinationEntity> DestinationEntities { get; set; }

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
            Database.EnsureDeleted();
            base.Dispose();
        }
    }
}