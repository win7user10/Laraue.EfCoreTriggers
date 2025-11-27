using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Linq2Triggers.Core.Converters.MethodCall.String.EndsWith
{
    /// <inheritdoc />
    public class StringEndsWithViaConcatFuncVisitor : BaseStringEndsWithVisitor
    {
        /// <inheritdoc />
        public StringEndsWithViaConcatFuncVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }
        
        /// <inheritdoc />
        protected override string BuildEndSql(string argumentSql)
        {
            return $"CONCAT('%', {argumentSql})";
        }
    }
}