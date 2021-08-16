using System;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Laraue.EfCoreTriggers.Common.Extensions
{
    public static class TriggerExtensions
    {
        private static Type _activeProviderType;

        /// <summary>
        /// Bad solution, but have no idea yet, how to register current provider using DbContextOptionsBuilder.
        /// </summary>
        public static void RememberTriggerProviderType<TTriggerProvider>()
            where TTriggerProvider : ITriggerProvider
        {
            _activeProviderType = typeof(TTriggerProvider);
        }

        public static ITriggerProvider GetSqlProvider(IModel model)
        {
            if (_activeProviderType is null)
            {
                throw new InvalidOperationException("To use triggers, DB provider should be added");
            }

            var providerConstructor = _activeProviderType.GetConstructor(new[] { typeof(IModel) });

            if (providerConstructor is null)
            {
                throw new InvalidOperationException("Provider should contain constructor with one parameter which receive instance of IModel");
            }

            var provider = providerConstructor.Invoke(new[]
            {
                (object) model
            });

            return (ITriggerProvider) provider;
        }
    }
}
