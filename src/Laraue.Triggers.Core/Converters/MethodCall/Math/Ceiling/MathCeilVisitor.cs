using Laraue.Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Triggers.Core.Converters.MethodCall.Math.Ceiling
{
    /// <inheritdoc />
    public class MathCeilVisitor : BaseMathCeilingVisitor
    {
        /// <inheritdoc />
        public MathCeilVisitor(IExpressionVisitorFactory visitorFactory) : base(visitorFactory)
        {
        }

        /// <inheritdoc />
        protected override string SqlFunctionName => "CEIL";
    }
}
