using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.Linq2Triggers.Core.TriggerBuilders.TableRefs;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Laraue.EfCoreTriggers.Tests.Tests.Base;

public abstract class BaseEfFunctionsTests
{
    /// <summary>
    /// EnumValue = Old.EnumValue
    /// </summary>
    protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> SetEfPropertyExpression =
        tableRefs => new DestinationEntity
        {
            IntValue = EF.Property<int>(tableRefs.New, "IntValue")
        };

    [Fact]
    public abstract void EfPropertyTranslationSql();
}