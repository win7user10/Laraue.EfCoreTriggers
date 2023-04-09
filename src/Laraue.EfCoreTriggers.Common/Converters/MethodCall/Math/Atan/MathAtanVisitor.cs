using Laraue.EfCoreTriggers.Common.SqlGeneration;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Atan
{
    /// <summary>
    /// Visitor for <see cref="System.Math.Atan"/> method.
    /// </summary>
    public class MathAtanVisitor : BaseMathVisitor
    {
        /// <inheritdoc />
        protected override string MethodName => nameof(System.Math.Atan);
        
        /// <inheritdoc />
        public MathAtanVisitor(IExpressionVisitorFactory visitorFactory)
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
            return SqlBuilder.FromString($"ATAN({sqlBuilder})");
        }
    }
}
