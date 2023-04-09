using Laraue.EfCoreTriggers.Common.SqlGeneration;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Exp
{
    /// <summary>
    /// Visitor for <see cref="System.Math.Exp"/> method.
    /// </summary>
    public class MathExpVisitor : BaseMathVisitor
    {
        /// <inheritdoc />
        protected override string MethodName => nameof(System.Math.Exp);

        /// <inheritdoc />
        public MathExpVisitor(IExpressionVisitorFactory visitorFactory) 
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
            return SqlBuilder.FromString($"EXP({sqlBuilder})");
        }
    }
}
