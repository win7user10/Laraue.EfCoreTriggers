using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.EndsWith
{
    public class StringEndsWithViaDoubleVerticalLineVisitor : BaseStringEndsWithVisitor
    {
        public StringEndsWithViaDoubleVerticalLineVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }
        
        protected override string BuildEndSql(string argumentSql)
        {
            return $"('%' || {argumentSql})";
        }
    }
}