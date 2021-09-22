namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Contains
{
    public class StringContainsViaCharindexFuncConverter : BaseStringContainsConverter
    {
        protected override string CombineSql(string expressionToSearchSql, string expressionToFindSql)
        {
            return $"CHARINDEX({expressionToFindSql}, {expressionToSearchSql}) > 0";
        }
    }
}