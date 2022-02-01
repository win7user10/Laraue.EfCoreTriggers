using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Trim
{
    public class StringTrimViaBtrimFuncVisitor : BaseStringTrimVisitor
    {
        public StringTrimViaBtrimFuncVisitor(IExpressionVisitorFactory visitorFactory)
            : base(visitorFactory)
        {
        }
        
        protected override string[] SqlTrimFunctionsNamesToApply { get; } = { "BTRIM" };
    }
}