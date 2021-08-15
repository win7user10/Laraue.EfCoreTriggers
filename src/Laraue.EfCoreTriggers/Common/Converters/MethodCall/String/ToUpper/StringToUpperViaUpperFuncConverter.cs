using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Builders.Providers;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.ToUpper
{
    public class ToUpperConverter : BaseStringConverter
    {
        /// <inheritdoc />
        public override string MethodName => nameof(string.ToUpper);

        /// <inheritdoc />
        public override SqlBuilder BuildSql(BaseExpressionProvider provider, MethodCallExpression expression, Dictionary<string, ArgumentType> argumentTypes)
        {
            var sqlBuilder = provider.GetExpressionSql(expression.Object, argumentTypes);
            return new(sqlBuilder.AffectedColumns, $"UPPER({sqlBuilder})");
        }
    }
}