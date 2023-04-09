using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Contains
{
    /// <inheritdoc />
    public class StringContainsViaInstrFuncVisitor : BaseStringContainsVisitor
    {
        /// <inheritdoc />
        public StringContainsViaInstrFuncVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }
        
        /// <inheritdoc />
        protected override string CombineSql(string expressionToSearchSql, string expressionToFindSql)
        {
            return $"INSTR({expressionToSearchSql}, {expressionToFindSql}) > 0";
        }
    }
}