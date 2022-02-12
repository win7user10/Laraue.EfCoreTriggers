using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Ceiling
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
