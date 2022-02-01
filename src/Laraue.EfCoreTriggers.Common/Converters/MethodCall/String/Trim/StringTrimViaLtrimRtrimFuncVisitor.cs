using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Trim
{
    public class StringTrimViaLtrimRtrimFuncVisitor : BaseStringTrimVisitor
    {
        protected override string[] SqlTrimFunctionsNamesToApply { get; } = { "LTRIM", "RTRIM" };
        
        public StringTrimViaLtrimRtrimFuncVisitor(IExpressionVisitorFactory visitorFactory)
            : base(visitorFactory)
        {
        }
    }
}