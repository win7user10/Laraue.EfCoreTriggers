using System.Linq.Expressions;
using Laraue.Triggers.Core.SqlGeneration;
using Laraue.Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Triggers.Core.Converters.MethodCall.Math.Cos
{
    /// <summary>
    /// Visitor for <see cref="System.Math.Cos"/> method.
    /// </summary>
    public class MathCosVisitor : BaseMathVisitor
    {
        /// <inheritdoc />
        protected override string MethodName => nameof(System.Math.Cos);

        /// <inheritdoc />
        public MathCosVisitor(IExpressionVisitorFactory visitorFactory) 
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
            return SqlBuilder.FromString($"COS({sqlBuilder})");
        }
    }
}
