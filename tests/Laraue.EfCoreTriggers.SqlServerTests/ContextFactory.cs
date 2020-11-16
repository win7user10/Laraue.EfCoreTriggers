using Laraue.EfCoreTriggers.Extensions;
using Laraue.EfCoreTriggers.Migrations;
using Laraue.EfCoreTriggers.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.SqlServerTests
{
    public class ContextFactory : BaseContextFactory<NativeDbContext>
    {
        public override NativeDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<NativeDbContext>()
                .UseSqlServer("Data Source=(LocalDb)\\v15.0;Initial Catalog=EfCoreTriggers",
                    x => x.MigrationsAssembly(typeof(ContextFactory).Assembly.FullName))
                .UseSnakeCaseNamingConvention()
                .UseTriggers()
                .Options;

            return new NativeDbContext(options);
        }
    }

    public class MyDesignTimeServices : IDesignTimeServices
    {
        public void ConfigureDesignTimeServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ICSharpHelper, CSharpHelper>();
        }
    }
}