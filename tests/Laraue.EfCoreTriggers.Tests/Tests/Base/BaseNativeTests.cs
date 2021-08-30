using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Xunit;

namespace Laraue.EfCoreTriggers.Tests.Tests.Base
{
    public abstract class BaseNativeTests
    {
        protected IContextOptionsFactory<DynamicDbContext> ContextOptionsFactory { get; }

        protected BaseNativeTests(IContextOptionsFactory<DynamicDbContext> contextOptionsFactory)
        {
            ContextOptionsFactory = contextOptionsFactory;
        }

        protected Task<DynamicDbContext> GetDbContextAsync(Expression<Func<SourceEntity, DestinationEntity>> insertDestinationEntityBasedOnSourceEntityFunc)
        {
            return DynamicDbContextFactory.GetDbContextAsync(
                ContextOptionsFactory,
                builder => builder.Entity<SourceEntity>()
                    .AfterInsert(trigger => trigger.Action(
                        action => action.Insert(insertDestinationEntityBasedOnSourceEntityFunc))));
        }
    }
}