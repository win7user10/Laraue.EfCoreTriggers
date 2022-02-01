using System;
using Laraue.EfCoreTriggers.MySql.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.Tests.Infrastructure;

public static class Helper
{
    public static T GetMySqlService<T>(ModelBuilder modelBuilder, Action<IServiceCollection> modifyServices = null)
    {
        return GetMySqlService<T>(modelBuilder.Model, modifyServices);
    }
    
    public static T GetMySqlService<T>(IReadOnlyModel model, Action<IServiceCollection> modifyServices = null)
    {
        var services = new ServiceCollection();
            
        services.AddMySqlServices()
            .AddSingleton(model);
        
        modifyServices?.Invoke(services);

        var provider = services.BuildServiceProvider();

        return provider.GetRequiredService<T>();
    }
}