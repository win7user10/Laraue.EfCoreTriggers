using System.Linq.Expressions;
using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Linq2Triggers.Core.Converters.MethodCall.Math.Asin
{
    /// <summary>
    /// Visitor for <see cref="System.Math.Asin"/> method.
    /// </summary>
    public class MathAsinVisitor : BaseMathVisitor
    {
        /// <inheritdoc />
        protected override string MethodName => nameof(System.Math.Asin);
        
        /// <inheritdoc />
        public MathAsinVisitor(IExpressionVisitorFactory visitorFactory)
            : base(visitorFactory)
        {
        }

        /// <inheritdoc />
        public override SqlBuilder Visit(
            MethodCallExpression expression,
            VisitedMembers visitedMembers)
        {
            var argument = expression.Arguments[0];
            var sqlBuilder = VisitorFactory.Visit(argument, visitedMembers);
            return SqlBuilder.FromString($"ASIN({sqlBuilder})");
        }
    }
}
