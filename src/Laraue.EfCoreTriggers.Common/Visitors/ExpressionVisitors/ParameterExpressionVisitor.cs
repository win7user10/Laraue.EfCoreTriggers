using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors
{
    /// <inheritdoc />
    public class ParameterExpressionVisitor : BaseExpressionVisitor<ParameterExpression>
    {
        private readonly ISqlGenerator _generator;

        public ParameterExpressionVisitor(ISqlGenerator generator)
        {
            _generator = generator;
        }

        /// <inheritdoc />
        public override SqlBuilder Visit(ParameterExpression expression, VisitedMembers visitedMembers)
        {
            return new SqlBuilder()
                .Append(_generator.GetTableSql(expression.Type));
        }
    }
}