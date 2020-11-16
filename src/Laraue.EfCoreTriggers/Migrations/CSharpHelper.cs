using Laraue.EfCoreTriggers.Common.Builders.Providers;
using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Laraue.EfCoreTriggers.Extensions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Laraue.EfCoreTriggers.Migrations
{
    public class CSharpHelper : Microsoft.EntityFrameworkCore.Design.Internal.CSharpHelper
    {
        private readonly IServiceProvider _provider;

        public CSharpHelper(IRelationalTypeMappingSource relationalTypeMappingSource, IServiceProvider provider)
            : base (relationalTypeMappingSource)
        {
            _provider = provider;
        }

        public override string UnknownLiteral(object value)
        {
            var triggerProviderIndo = _provider.GetRequiredService<TriggerInitializeInfo>();

            if (value is ISqlConvertible sqlConvertible)
            {
                // return sqlConvertible.BuildSql(_provider);
            }

            return base.UnknownLiteral(value);
        }
    }
}
