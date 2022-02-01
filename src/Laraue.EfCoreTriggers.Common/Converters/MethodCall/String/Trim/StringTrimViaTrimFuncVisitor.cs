using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Trim
{
    public class StringTrimViaTrimFuncVisitor : BaseStringTrimVisitor
    {
        protected override string[] SqlTrimFunctionsNamesToApply { get; } = { "TRIM" };
        
        public StringTrimViaTrimFuncVisitor(IExpressionVisitorFactory visitorFactory)
            : base(visitorFactory)
        {
        }
    }
}