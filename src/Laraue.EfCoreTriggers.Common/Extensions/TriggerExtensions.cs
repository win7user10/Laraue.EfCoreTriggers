using Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.Common.Extensions
{
    public static class TriggerExtensions
    {
        public static readonly IServiceCollection Services = new ServiceCollection();

        public static ITriggerVisitor GetVisitor(IReadOnlyModel model)
        {
            var services = Services.AddSingleton(model);

            var provider = services.BuildServiceProvider();

            return provider.GetRequiredService<ITriggerVisitor>();
        }
    }
}
