using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Trim
{
    public abstract class BaseStringTrimConverter : BaseStringConverter
    {
        /// <summary>Sequence of SQL functions which should be </summary>
        protected abstract string[] SqlTrimFunctionsNamesToApply { get; }
        
        /// <inheritdoc />
        public override string MethodName => nameof(string.Trim);

        /// <inheritdoc />
        public override SqlBuilder BuildSql(BaseExpressionProvider provider, MethodCallExpression expression, Dictionary<string, ArgumentType> argumentTypes)
        {
            var expressionSqlBuilder = provider.GetExpressionSql(expression.Object, argumentTypes);
            
            var sqlBuilder = new SqlBuilder(expressionSqlBuilder.AffectedColumns);

            foreach (var trimFunctionName in SqlTrimFunctionsNamesToApply)
            {
                sqlBuilder.Append(trimFunctionName).Append('(');
            }

            sqlBuilder.Append(expressionSqlBuilder);

            foreach (var _ in SqlTrimFunctionsNamesToApply)
            {
                sqlBuilder.Append(')');
            }

            return sqlBuilder;
        }
    }
}