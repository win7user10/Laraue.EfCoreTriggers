using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Cos
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
            ArgumentTypes argumentTypes,
            VisitedMembers visitedMembers)
        {
            var argument = expression.Arguments[0];
            var sqlBuilder = VisitorFactory.Visit(argument, argumentTypes, visitedMembers);
            return SqlBuilder.FromString($"COS({sqlBuilder})");
        }
    }
}
