namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Contains
{
    public class StringContainsViaStrposFuncConverter : BaseStringContainsConverter
    {
        protected override string CombineSql(string expressionToSearchSql, string expressionToFindSql)
        {
            return $"STRPOS({expressionToSearchSql}, {expressionToFindSql}) > 0";
        }
    }
}