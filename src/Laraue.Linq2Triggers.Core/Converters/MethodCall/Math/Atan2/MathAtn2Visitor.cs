using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Linq2Triggers.Core.Converters.MethodCall.Math.Atan2
{
    /// <inheritdoc />
    public class MathAtn2Visitor : BaseAtan2Visitor
    {
        /// <inheritdoc />
        protected override string SqlFunctionName => "ATN2";
        
        /// <inheritdoc />
        public MathAtn2Visitor(IExpressionVisitorFactory visitorFactory)
            : base(visitorFactory)
        {
        }
    }
}
