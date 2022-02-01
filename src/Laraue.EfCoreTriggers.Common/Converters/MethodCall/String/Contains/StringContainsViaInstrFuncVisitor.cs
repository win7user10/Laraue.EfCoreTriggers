using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Contains
{
    public class StringContainsViaInstrFuncVisitor : BaseStringContainsVisitor
    {
        public StringContainsViaInstrFuncVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }
        
        protected override string CombineSql(string expressionToSearchSql, string expressionToFindSql)
        {
            return $"INSTR({expressionToSearchSql}, {expressionToFindSql}) > 0";
        }
    }
}