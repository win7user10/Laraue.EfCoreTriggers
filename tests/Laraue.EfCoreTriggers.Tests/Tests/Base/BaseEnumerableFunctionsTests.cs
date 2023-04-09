using System;
using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;
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
    protected readonly Expression<Func<OldAndNewTableRefs<SourceEntity>, DestinationEntity>> CountRelatedExpression = refs => new DestinationEntity
    {
        IntValue = refs.New.RelatedEntities.Count(),
        BooleanValue = refs.New.BooleanValue,
    };

    [Fact]
    public abstract void CountRelatedSql();
    
    /// <summary>
    /// IntValue = SELECT count(*) FROM destination_entities WHERE destination_entities.int_value > 1
    /// </summary>
    protected readonly Expression<Func<OldAndNewTableRefs<SourceEntity>, DestinationEntity>> CountRelatedWithPredicateExpression = (tableRefs) => new DestinationEntity
    {
        IntValue = tableRefs.New.RelatedEntities.Count(x => x.IntValue > 1)
    };


    [Fact]
    public abstract void CountRelatedWithPredicateSql();
    
    /// <summary>
    /// IntValue = SELECT count(*) FROM destination_entities WHERE destination_entities.int_value
    /// more than 1 AND destination_entities.int_value less than 3
    /// </summary>
    protected readonly Expression<Func<OldAndNewTableRefs<SourceEntity>, DestinationEntity>> CountRelatedWithWherePredicateExpression = tableRefs
        => new DestinationEntity
        {
            IntValue = tableRefs.New.RelatedEntities
                .Where(x => x.IntValue < 3)
                .Count(x => x.IntValue > 1),
        };

    [Fact]
    public abstract void CountRelatedWithWherePredicateSql();
}