using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Builders.Providers;
using Laraue.EfCoreTriggers.Extensions;

namespace Laraue.EfCoreTriggers.Common.Converters.ExpressionCall.String
{
    public class ConcatConverter : BaseStringConverter
    {
        /// <inheritdoc />
        public override string MethodName => nameof(string.Concat);

        /// <inheritdoc />
        public override SqlBuilder BuildSql(BaseExpressionProvider provider, MethodCallExpression expression, Dictionary<string, ArgumentType> argumentTypes)
        {
            var concatExpressionArgsSql = provider.GetMethodCallArgumentsSql(expression, argumentTypes);

            return new SqlBuilder(concatExpressionArgsSql)
                .AppendJoin(" + ", concatExpressionArgsSql.Select(x => x.StringBuilder));
        }
    }
}