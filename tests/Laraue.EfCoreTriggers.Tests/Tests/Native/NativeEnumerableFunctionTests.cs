using System;
using System.Collections.Generic;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Base;
using Xunit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.Tests.Tests.Native;

[IntegrationTest]
public class NativeEnumerableFunctionTests : BaseEnumerableFunctionsTests
{
    protected IContextOptionsFactory<DynamicDbContext> ContextOptionsFactory { get; }

    protected NativeEnumerableFunctionTests(IContextOptionsFactory<DynamicDbContext> contextOptionsFactory)
    {
        ContextOptionsFactory = contextOptionsFactory;
    }

    public override void CountRelatedSql()
    {
        using var dbContext = DynamicDbContextFactory.GetDbContext(
            ContextOptionsFactory,
            builder =>
            {
                builder.Entity<SourceEntity>()
                    .AfterUpdate(trigger => trigger.Action(
                        action => action.Insert(CountRelatedExpression)));
            });

        var sourceEntity = new SourceEntity
        {
            RelatedEntities = new List<RelatedEntity>()
            {
                new(),
                new(),
                new(),
            }
        };

        dbContext.SourceEntities.Add(new SourceEntity
        {
            RelatedEntities = new List<RelatedEntity>
            {
                new(),
            }
        });
        
        dbContext.SourceEntities.Add(sourceEntity);
        dbContext.SaveChanges();

        sourceEntity.DecimalValue = 20;
        dbContext.SaveChanges();

        var insertedEntity = Assert.Single(dbContext.DestinationEntities);
        Assert.Equal(3, insertedEntity.IntValue);
    }

    public override void CountRelatedWithPredicateSql()
    {
        throw new NotImplementedException();
    }
}