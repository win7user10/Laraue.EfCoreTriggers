using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.Common.Extensions
{
    /// <summary>
    /// EFCore triggers extensions for initializing.  
    /// </summary>
    public static class TriggerExtensions
    {
        /// <summary>
        /// Services which will be used to create EFCore triggers SQL provider.
        /// </summary>
        public static readonly IServiceCollection Services = new ServiceCollection();

        /// <summary>
        /// Build EFCore triggers provider based on configured <see cref="Services"/>
        /// and passed <see cref="IReadOnlyModel"/>.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ITriggerVisitor GetVisitor(IReadOnlyModel model)
        {
            var services = Services.AddSingleton(model);

            var provider = services.BuildServiceProvider();

            return provider.GetRequiredService<ITriggerVisitor>();
        }
    }
}
