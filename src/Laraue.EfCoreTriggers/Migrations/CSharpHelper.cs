using Laraue.EfCoreTriggers.Common.Builders.Providers;
using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace Laraue.EfCoreTriggers.Migrations
{
    public class CSharpHelper : Microsoft.EntityFrameworkCore.Design.Internal.CSharpHelper
    {
        private readonly ITriggerProvider _provider;

        public CSharpHelper(IRelationalTypeMappingSource relationalTypeMappingSource, ITriggerProvider provider)
            : base (relationalTypeMappingSource)
        {
            _provider = provider;
        }

        public override string UnknownLiteral(object value)
        {
            if (value is ISqlConvertible sqlConvertible)
            {
                return sqlConvertible.BuildSql(_provider);
            }

            return base.UnknownLiteral(value);
        }
    }
}
