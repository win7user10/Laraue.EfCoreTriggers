using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Contains
{
    public class StringContainsViaCharindexFuncVisitor : BaseStringContainsVisitor
    {
        public StringContainsViaCharindexFuncVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }
        
        protected override string CombineSql(string expressionToSearchSql, string expressionToFindSql)
        {
            return $"CHARINDEX({expressionToFindSql}, {expressionToSearchSql}) > 0";
        }
    }
}