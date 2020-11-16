using Laraue.EfCoreTriggers.Common;
using Laraue.EfCoreTriggers.Common.Builders.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Laraue.EfCoreTriggers.Extensions
{
    public class TriggersExtension : IDbContextOptionsExtension
    {
        DbContextOptions _dbContextOptions;

        private static readonly Dictionary<string, DbProvider> ProviderOptionTypes = new Dictionary<string, DbProvider>
        {
            ["NpgsqlOptionsExtension"] = DbProvider.PostgreSql,
            ["SqlServerOptionsExtension"] = DbProvider.SqlServer,
        };

        public TriggersExtension(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
                throw new InvalidOperationException("To use triggers, DB provider should be added");

            _dbContextOptions = builder.Options;
        }

        private DbProvider GetProvider()
        {
            var dbProviderOptions = _dbContextOptions.Extensions.Where(x => x.Info.IsDatabaseProvider)
                .SingleOrDefault();

            var dbProviderOptionsType = dbProviderOptions.GetType().Name;

            if (ProviderOptionTypes.TryGetValue(dbProviderOptionsType, out var provider))
            {
                return provider;
            }

            throw new NotSupportedException($"Provider with options {dbProviderOptionsType} is not supported.");
        }

        public DbContextOptionsExtensionInfo Info => new DbContextOptionsExtensionInfoTrigger(this);

        public void ApplyServices(IServiceCollection services)
        {
            services.AddSingleton(new TriggerInitializeInfo { DbProvider = GetProvider() });
        }

        public void Validate(IDbContextOptions options)
        {
        }
    }

    public class DbContextOptionsExtensionInfoTrigger : DbContextOptionsExtensionInfo
    {
        public DbContextOptionsExtensionInfoTrigger(IDbContextOptionsExtension extension)
            : base(extension)
        {
        }

        public override bool IsDatabaseProvider => false;

        public override string LogFragment => "EfCore triggers";

        public override long GetServiceProviderHashCode()
            => Extension.GetHashCode();

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
        }
    }
}
