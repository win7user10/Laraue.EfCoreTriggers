using Laraue.EfCoreTriggers.Migrations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

[assembly: DesignTimeProviderServices("Laraue.EfCoreTriggers.Design.TriggersDesignTimeServices")]
namespace Laraue.EfCoreTriggers.Design
{
    public class TriggersDesignTimeServices : IDesignTimeServices
    {
        public void ConfigureDesignTimeServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ICSharpHelper, CSharpHelper>();
        }
    }
}
