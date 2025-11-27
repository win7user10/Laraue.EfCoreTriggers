using Laraue.Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Triggers.Core.Converters.MethodCall.Math.Ceiling
{
    /// <inheritdoc />
    public class MathCeilingVisitor : BaseMathCeilingVisitor
    {
        /// <inheritdoc />
        public MathCeilingVisitor(IExpressionVisitorFactory visitorFactory) : base(visitorFactory)
        {
        }

        /// <inheritdoc />
        protected override string SqlFunctionName => "CEILING";
    }
}
