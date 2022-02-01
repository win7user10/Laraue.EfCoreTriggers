using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Contains
{
    public class StringContainsViaStrposFuncVisitor : BaseStringContainsVisitor
    {
        public StringContainsViaStrposFuncVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }
        
        protected override string CombineSql(string expressionToSearchSql, string expressionToFindSql)
        {
            return $"STRPOS({expressionToSearchSql}, {expressionToFindSql}) > 0";
        }
    }
}