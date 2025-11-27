using System.Linq.Expressions;
using Laraue.Triggers.Core.Extensions;
using Laraue.Triggers.Core.SqlGeneration;
using Laraue.Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Triggers.Core.Converters.MethodCall.String.EndsWith
{
    /// <summary>
    /// Visitor for <see cref="System.String.EndsWith(string)"/> method.
    /// </summary>
    public abstract class BaseStringEndsWithVisitor : BaseStringVisitor
    {
        /// <inheritdoc />
        protected BaseStringEndsWithVisitor(IExpressionVisitorFactory visitorFactory)
            : base(visitorFactory)
        {
        }
        
        /// <inheritdoc />
        protected override string MethodName => nameof(string.EndsWith);

        /// <inheritdoc />
        public override SqlBuilder Visit(
            MethodCallExpression expression,
            VisitedMembers visitedMembers)
        {
            var argumentSql = VisitorFactory.VisitArguments(expression, visitedMembers)[0];
            
            var sqlBuilder = VisitorFactory.Visit(expression.Object, visitedMembers);
            
            return SqlBuilder.FromString($"{sqlBuilder} LIKE {BuildEndSql(argumentSql)}");
        }

        /// <summary>
        /// Build end SQL expression from argument SQL expression.
        /// </summary>
        /// <param name="argumentSql"></param>
        /// <returns></returns>
        protected abstract string BuildEndSql(string argumentSql);
    }
}