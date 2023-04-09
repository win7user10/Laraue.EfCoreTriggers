using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Atan2
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
