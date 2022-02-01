using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Atan2
{
    public class MathAtan2Visitor : BaseAtan2Visitor
    {
        protected override string SqlFunctionName => "ATAN2";
        
        public MathAtan2Visitor(IExpressionVisitorFactory visitorFactory)
            : base(visitorFactory)
        {
        }
    }
}
