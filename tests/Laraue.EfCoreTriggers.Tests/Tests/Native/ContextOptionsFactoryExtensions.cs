using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;

namespace Laraue.EfCoreTriggers.Tests.Tests.Native
{
    public static class ContextOptionsFactoryExtensions
    {
        public static DestinationEntity ExecuteTest(this IContextOptionsFactory<DynamicDbContext> contextOptionsFactory, Expression<Func<SourceEntity, DestinationEntity>> triggerExpression, SourceEntity source)
        {
            using var dbContext = contextOptionsFactory.GetDbContext(triggerExpression);

            dbContext.SourceEntities.Add(source);
            dbContext.SaveChanges();

            return Assert.Single(dbContext.DestinationEntities);
        }
    }
}