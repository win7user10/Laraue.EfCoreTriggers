using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Laraue.EfCoreTriggers.Tests.Tests.Native.TriggerTests
{
    public class BaseTriggerTests
    {
        private readonly IContextOptionsFactory<DynamicDbContext> _contextOptionsFactory;

        protected BaseTriggerTests(IContextOptionsFactory<DynamicDbContext> contextOptionsFactory)
        {
            _contextOptionsFactory = contextOptionsFactory;
        }
        
        protected DynamicDbContext CreateDbContext(Action<EntityTypeBuilder<SourceEntity>> setupModelBuilder)
        {
            return DynamicDbContextFactory.GetDbContext(
                _contextOptionsFactory, builder =>
                {
                    setupModelBuilder.Invoke(builder.Entity<SourceEntity>());
                });
        }
    }

    internal static class DbContextExtensions
    {
        public static void Save(this DbContext context, params object[] entities)
        {
            context.AddRange(entities);
            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }
}