using System;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Base;
using Microsoft.EntityFrameworkCore;

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
}