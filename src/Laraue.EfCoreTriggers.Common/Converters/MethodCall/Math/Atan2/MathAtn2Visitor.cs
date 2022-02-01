using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Atan2
{
    public class MathAtn2Visitor : BaseAtan2Visitor
    {
        protected override string SqlFunctionName => "ATN2";
        
        public MathAtn2Visitor(IExpressionVisitorFactory visitorFactory)
            : base(visitorFactory)
        {
        }
    }
}
