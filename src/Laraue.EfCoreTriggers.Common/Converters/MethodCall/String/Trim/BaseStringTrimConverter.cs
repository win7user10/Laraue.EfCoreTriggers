using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Internal;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Trim
{
    public abstract class BaseStringTrimConverter : BaseStringConverter
    {
        /// <summary>Sequence of SQL functions which should be </summary>
        protected abstract string[] SqlTrimFunctionsNamesToApply { get; }
        
        /// <inheritdoc />
        protected override string MethodName => nameof(string.Trim);

        /// <inheritdoc />
        public override SqlBuilder BuildSql(
            IExpressionVisitor visitor,
            MethodCallExpression expression,
            ArgumentTypes argumentTypes,
            VisitedMembers visitedMembers)
        {
            var expressionSqlBuilder = visitor.Visit(expression.Object, argumentTypes, visitedMembers);
            
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