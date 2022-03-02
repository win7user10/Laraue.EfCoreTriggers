using System;
using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Xunit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.Tests.Tests.Base;

[UnitTest]
public abstract class BaseEnumerableFunctionsTests
{
    /// <summary>
    /// SELECT (SELECT COUNT(*) FROM related_entities WHERE source_entities.id = NEW.SourceEntityId), NEW.boolean_value
    /// </summary>
    protected Expression<Func<SourceEntity, SourceEntity, DestinationEntity>> CountRelatedExpression = (prev, next) => new DestinationEntity
    {
        IntValue = next.RelatedEntities.Count(),
        BooleanValue = next.BooleanValue,
    };

    [Fact]
    public abstract void CountRelatedSql();
    
    /// <summary>
    /// IntValue = SELECT count(*) FROM destination_entities WHERE destination_entities.int_value > 1
    /// </summary>
    protected Expression<Func<SourceEntity, SourceEntity, DestinationEntity>> CountRelatedWithPredicateExpression = (prev, next) => new DestinationEntity
    {
        IntValue = next.RelatedEntities.Count(x => x.IntValue > 1)
    };


    [Fact]
    public abstract void CountRelatedWithPredicateSql();
    
    /// <summary>
    /// IntValue = SELECT count(*) FROM destination_entities WHERE destination_entities.int_value
    /// more than 1 AND destination_entities.int_value less than 3
    /// </summary>
    protected Expression<Func<SourceEntity, SourceEntity, DestinationEntity>> CountRelatedWithWherePredicateExpression = (prev, next) => new DestinationEntity
    {
        IntValue = next.RelatedEntities
            .Where(x => x.IntValue < 3)
            .Count(x => x.IntValue > 1),
    };

    [Fact]
    public abstract void CountRelatedWithWherePredicateSql();
}