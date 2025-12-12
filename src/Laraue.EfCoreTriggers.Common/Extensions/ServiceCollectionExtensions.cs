using Laraue.EfCoreTriggers.Common.Converters;
using Laraue.EfCoreTriggers.Common.Migrations;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.Linq2Triggers.Core.Extensions;
using Laraue.Linq2Triggers.Core.SqlGeneration;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.Common.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add EF Core triggers MySQL provider services.
    /// </summary>
    public static IServiceCollection AddEfCoreTriggerAdapters(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddMethodCallConverter<EfMethodCallVisitor>()
            .AddScoped<IDbSchemaRetriever, EfCoreDbSchemaRetriever>()
            .AddScoped<ITriggerModelDiffer, TriggerModelDiffer>();
    }
}