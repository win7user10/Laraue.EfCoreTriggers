using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.SqlGeneration;
using Laraue.EfCoreTriggers.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Converters.MethodCall
{
    public abstract class MethodCallConverter : IMethodCallConverter
    {
        /// <inheritdoc />
        public abstract bool IsApplicable(MethodCallExpression expression);

        /// <inheritdoc />
        public abstract SqlBuilder BuildSql(BaseExpressionProvider provider, MethodCallExpression expression,
            Dictionary<string, ArgumentType> argumentTypes);
    }
}