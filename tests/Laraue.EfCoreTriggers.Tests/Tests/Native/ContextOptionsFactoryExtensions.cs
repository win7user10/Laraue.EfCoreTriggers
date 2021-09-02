using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
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
        /// <param name="source">Entity to insert in the table with <see cref="SourceEntity"/></param>
        /// <returns>Entity inserted by trigger</returns>
        public static DestinationEntity CheckTrigger(this IContextOptionsFactory<DynamicDbContext> contextOptionsFactory, Expression<Func<SourceEntity, DestinationEntity>> triggerExpression, SourceEntity source)
        {
            using var dbContext = contextOptionsFactory.GetDbContext(triggerExpression);

            dbContext.SourceEntities.Add(source);
            dbContext.SaveChanges();

            return Assert.Single(dbContext.DestinationEntities);
        }
    }
}