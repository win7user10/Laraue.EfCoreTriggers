using System;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Base;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Laraue.EfCoreTriggers.Tests.Tests.Native;

public abstract class NativeEFFunctionsTests : BaseEfFunctionsTests
{
    protected IContextOptionsFactory<DynamicDbContext> ContextOptionsFactory { get; }
    protected Action<DynamicDbContext> SetupDbContext { get; }
    protected Action<ModelBuilder> SetupModelBuilder { get; }

    protected NativeEFFunctionsTests(
        IContextOptionsFactory<DynamicDbContext> contextOptionsFactory, 
        Action<DynamicDbContext> setupDbContext = null,
        Action<ModelBuilder> setupModelBuilder = null)
    {
        ContextOptionsFactory = contextOptionsFactory;
        SetupDbContext = setupDbContext;
        SetupModelBuilder = setupModelBuilder;
    }

    public override void EfPropertyTranslationSql()
    {
        var insertedEntity = ContextOptionsFactory.CheckTrigger(SetEfPropertyExpression, SetupDbContext, SetupModelBuilder, new SourceEntity
        {
            IntValue = 23
        });
        
        Assert.Equal(23, insertedEntity.IntValue);
    }
}