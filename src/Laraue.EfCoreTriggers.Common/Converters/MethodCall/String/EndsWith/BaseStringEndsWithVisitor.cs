using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.EndsWith
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