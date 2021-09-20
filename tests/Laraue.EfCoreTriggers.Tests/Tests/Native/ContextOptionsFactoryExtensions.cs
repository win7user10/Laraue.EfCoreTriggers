using System;
using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Laraue.EfCoreTriggers.Tests.Tests.Native
{
    public static class ContextOptionsFactoryExtensions
    {
        /// <summary>
        /// Creates new database with the trigger, then insert into a database passed <see cref="SourceEntity"/>
        /// and return inserted through trigger <see cref="DestinationEntity"/>.
        /// </summary>
        /// <param name="contextOptionsFactory">Factory to initialize new <see cref="DynamicDbContext"/></param>
        /// <param name="triggerExpression">Expression which describe how to create entity in the table with <see cref="DestinationEntity"/>
        ///     basing on passed <see cref="SourceEntity"/></param>
        /// <param name="setupDbContext">Actions with DbContext before a test will start</param>
        /// <param name="setupModelBuilder">Actions with DbContext model builder before a test will start</param>
        /// <param name="sourceEntities">Entities to insert in the table</param>
        /// <returns>Entity inserted by trigger</returns>
        public static DestinationEntity[] CheckTrigger(
            this IContextOptionsFactory<DynamicDbContext> contextOptionsFactory, 
            Expression<Func<SourceEntity, DestinationEntity>> triggerExpression,
            Action<DynamicDbContext> setupDbContext,
            Action<ModelBuilder> setupModelBuilder,
            params SourceEntity[] sourceEntities)
        {
            using var dbContext = DynamicDbContextFactory.GetDbContext(
                contextOptionsFactory, builder =>
                {
                    builder.Entity<SourceEntity>()
                        .AfterInsert(trigger => trigger.Action(
                            action => action.Insert(triggerExpression)));
                    setupModelBuilder?.Invoke(builder);
                });

            setupDbContext?.Invoke(dbContext);
            dbContext.SourceEntities.AddRange(sourceEntities);
            dbContext.SaveChanges();

            return dbContext.DestinationEntities.ToArray();
        }

        public static DestinationEntity CheckTrigger(
            this IContextOptionsFactory<DynamicDbContext> contextOptionsFactory,
            Expression<Func<SourceEntity, DestinationEntity>> triggerExpression,
            Action<DynamicDbContext> setupDbContext,
            Action<ModelBuilder> setupModelBuilder,
            SourceEntity source)
        {
            return Assert.Single(contextOptionsFactory.CheckTrigger(triggerExpression, setupDbContext, setupModelBuilder, new[] { source }));
        }
    }
}