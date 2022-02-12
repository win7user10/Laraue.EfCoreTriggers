using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Trim
{
    /// <summary>
    /// Visitor for <see cref="System.String.Trim()"/> method.
    /// </summary>
    public abstract class BaseStringTrimVisitor : BaseStringVisitor
    {
        /// <summary>
        /// Sequence of SQL functions which should be applied to execute string trim.
        /// </summary>
        protected abstract IEnumerable<string> SqlTrimFunctionsNamesToApply { get; }
        
        /// <inheritdoc />
        protected override string MethodName => nameof(string.Trim);

        /// <inheritdoc />
        protected BaseStringTrimVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }

        /// <inheritdoc />
        public override SqlBuilder Visit(
            MethodCallExpression expression,
            ArgumentTypes argumentTypes,
            VisitedMembers visitedMembers)
        {
            var expressionSqlBuilder = VisitorFactory.Visit(expression.Object, argumentTypes, visitedMembers);
            
            var sqlBuilder = new SqlBuilder();

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