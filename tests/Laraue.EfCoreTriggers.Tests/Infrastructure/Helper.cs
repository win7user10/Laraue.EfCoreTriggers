using System;
using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.Tests.Infrastructure;

public static class Helper
{
    public static T GetService<T>(ModelBuilder modelBuilder, Action<IServiceCollection> modifyServices = null)
    {
        return GetService<T>(modelBuilder.Model, modifyServices);
    }
    
    public static T GetService<T>(IReadOnlyModel model, Action<IServiceCollection> modifyServices = null)
    {
        var services = new ServiceCollection();
        
        services.AddSingleton(model);
        
        modifyServices?.Invoke(services);

        var provider = services.BuildServiceProvider();

        return provider.GetRequiredService<T>();
    }
    
    public static ITriggerActionVisitorFactory GetTriggerActionFactory(IReadOnlyModel model, Action<IServiceCollection> modifyServices = null)
    {
        return GetService<ITriggerActionVisitorFactory>(model, modifyServices);
    }
}