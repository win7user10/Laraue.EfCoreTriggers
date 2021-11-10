using System;
using Laraue.EfCoreTriggers.Common.Converters;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Laraue.EfCoreTriggers.Common.Extensions
{
    public static class TriggerExtensions
    {
        private static Type _activeProviderType;
        private static Action<AvailableConverters> _setupProviderConverters;

        /// <summary>
        /// Bad solution, but have no idea yet, how to register current provider using DbContextOptionsBuilder.
        /// </summary>
        public static void RememberTriggerProvider<TTriggerProvider>(Action<AvailableConverters> setupConverters)
            where TTriggerProvider : ITriggerProvider
        {
            _activeProviderType = typeof(TTriggerProvider);
            _setupProviderConverters = setupConverters;
        }

        public static ITriggerProvider GetSqlProvider(IReadOnlyModel model)
        {
            if (_activeProviderType is null)
            {
                throw new InvalidOperationException("To use triggers, DB provider should be added");
            }

            var providerConstructor = _activeProviderType.GetConstructor(new[] { typeof(IReadOnlyModel) });

            if (providerConstructor is null)
            {
                throw new InvalidOperationException("Provider should contain constructor with one parameter which receive instance of IModel");
            }

            var provider = (ITriggerProvider)providerConstructor.Invoke(new[]
            {
                (object) model
            });

            _setupProviderConverters?.Invoke(provider.Converters);

            return provider;
        }
    }
}
