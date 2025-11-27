using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Linq2Triggers.Core.Converters.MethodCall.String.Contains
{
    /// <inheritdoc />
    public class StringContainsViaStrposFuncVisitor : BaseStringContainsVisitor
    {
        /// <inheritdoc />
        public StringContainsViaStrposFuncVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }
        
        /// <inheritdoc />
        protected override string CombineSql(string expressionToSearchSql, string expressionToFindSql)
        {
            return $"STRPOS({expressionToSearchSql}, {expressionToFindSql}) > 0";
        }
    }
}