using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Extensions;
using Laraue.EfCoreTriggers.SqlGeneration;
using Laraue.EfCoreTriggers.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Converters.MethodCall.String.Concat
{
    public abstract class BaseStringConcatConverter : BaseStringConverter
    {
        /// <inheritdoc />
        public override string MethodName => nameof(string.Concat);

        /// <inheritdoc />
        public override SqlBuilder BuildSql(BaseExpressionProvider provider, MethodCallExpression expression, Dictionary<string, ArgumentType> argumentTypes)
        {
            var argumentsSql = provider.GetMethodCallArgumentsSql(expression, argumentTypes);

            return BuildSql(argumentsSql);
        }

        public abstract SqlBuilder BuildSql(SqlBuilder[] argumentsSql);
    }
}