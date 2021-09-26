using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        
        public static void Delete<TEntity, TDbContext>(this TDbContext dbContext, 
            Func<TDbContext, DbSet<TEntity>> dbSetGetter, Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
            where TDbContext : DbContext
        {
            var dbSet = dbSetGetter.Invoke(dbContext);
            var entities = dbSet.Where(predicate).ToArray();
            foreach (var entity in entities)
            {
                dbSet.Remove(entity);
            }
            
            dbContext.SaveChanges();
            dbContext.ChangeTracker.Clear();
        }
        
        public static void Delete<TEntity, TDbContext>(this TDbContext dbContext, 
            Func<TDbContext, DbSet<TEntity>> dbSetGetter)
            where TEntity : class
            where TDbContext : DbContext
        {
            Delete<TEntity, TDbContext>(dbContext, dbSetGetter, _ => true);
        }
        
        public static void Update<TEntity, TDbContext>(
            this TDbContext dbContext, 
            Func<TDbContext, DbSet<TEntity>> dbSetGetter, 
            Expression<Func<TEntity, bool>> predicate,
            Action<TEntity> changeEntity)
            where TEntity : class
            where TDbContext : DbContext
        {
            var dbSet = dbSetGetter.Invoke(dbContext); 
            var entities = dbSet.Where(predicate)
                .ToArray();

            foreach (var entity in entities)
            {
                changeEntity(entity);
            }

            dbContext.SaveChanges();
            dbContext.ChangeTracker.Clear();
        }
        
        public static void Update<TEntity, TDbContext>(
            this TDbContext dbContext, 
            Func<TDbContext, DbSet<TEntity>> dbSetGetter,
            Action<TEntity> changeEntity)
            where TEntity : class
            where TDbContext : DbContext
        {
            Update(dbContext, dbSetGetter, x => true, changeEntity);
        }
    }
}