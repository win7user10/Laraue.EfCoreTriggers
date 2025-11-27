using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Linq2Triggers.Core.Converters.MethodCall.String.EndsWith
{
    /// <inheritdoc />
    public class StringEndsWithViaDoubleVerticalLineVisitor : BaseStringEndsWithVisitor
    {
        /// <inheritdoc />
        public StringEndsWithViaDoubleVerticalLineVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }
        
        /// <inheritdoc />
        protected override string BuildEndSql(string argumentSql)
        {
            return $"('%' || {argumentSql})";
        }
    }
}