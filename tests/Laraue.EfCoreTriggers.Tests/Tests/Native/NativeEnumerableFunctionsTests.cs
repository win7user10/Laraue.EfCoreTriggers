using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Base;
using Xunit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.Tests.Tests.Native;

[IntegrationTest]
public abstract class NativeEnumerableFunctionsTests : BaseEnumerableFunctionsTests
{
    private IContextOptionsFactory<DynamicDbContext> ContextOptionsFactory { get; }

    protected NativeEnumerableFunctionsTests(IContextOptionsFactory<DynamicDbContext> contextOptionsFactory)
    {
        ContextOptionsFactory = contextOptionsFactory;
    }

    private void Test(Expression<Func<OldAndNewTableRefs<SourceEntity>, DestinationEntity>> expression, Action<DynamicDbContext, SourceEntity> testAction)
    {
        using var dbContext = DynamicDbContextFactory.GetDbContext(
            ContextOptionsFactory,
            builder =>
            {
                builder.Entity<SourceEntity>()
                    .AfterUpdate(trigger => trigger.Action(
                        action => action.Insert(expression)));
            });

        var sourceEntity = new SourceEntity
        {
            RelatedEntities = new List<RelatedEntity>()
            {
                new() { IntValue = 1 },
                new() { IntValue = 2 },
                new() { IntValue = 3 },
            }
        };

        dbContext.SourceEntities.Add(new SourceEntity
        {
            RelatedEntities = new List<RelatedEntity>
            {
                new() { IntValue = 2 },
            }
        });
        
        dbContext.SourceEntities.Add(sourceEntity);
        dbContext.SaveChanges();

        testAction(dbContext, sourceEntity);
    }

    public override void CountRelatedSql()
    {
        Test(CountRelatedExpression, (context, entity) =>
        {
            entity.DecimalValue = 20;
            context.SaveChanges();
            
            var insertedEntity = Assert.Single(context.DestinationEntities);
            Assert.Equal(3, insertedEntity.IntValue);
        });
    }

    public override void CountRelatedWithPredicateSql()
    {
        Test(CountRelatedWithPredicateExpression, (context, entity) =>
        {
            entity.DecimalValue = 20;
            context.SaveChanges();
            
            var insertedEntity = Assert.Single(context.DestinationEntities);
            Assert.Equal(2, insertedEntity.IntValue);
        });
    }

    public override void CountRelatedWithWherePredicateSql()
    {
        Test(CountRelatedWithWherePredicateExpression, (context, entity) =>
        {
            entity.DecimalValue = 20;
            context.SaveChanges();
            
            var insertedEntity = Assert.Single(context.DestinationEntities);
            Assert.Equal(1, insertedEntity.IntValue);
        });
    }
}