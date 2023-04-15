using System;
using Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.Tests.Infrastructure;

public static class Helper
{
    public static T GetService<T>(ModelBuilder modelBuilder, Action<IServiceCollection> modifyServices = null)
    {
        return GetService<T>(modelBuilder.Model.FinalizeModel(), modifyServices);
    }
    
    public static T GetService<T>(IModel model, Action<IServiceCollection> modifyServices = null)
    {
        var services = new ServiceCollection();
        
        services.AddScoped(_ => model);
        
        modifyServices?.Invoke(services);

        var provider = services.BuildServiceProvider();

        return provider.GetRequiredService<T>();
    }
    
    public static ITriggerActionVisitorFactory GetTriggerActionFactory(IModel model, Action<IServiceCollection> modifyServices = null)
    {
        return GetService<ITriggerActionVisitorFactory>(model, modifyServices);
    }
}