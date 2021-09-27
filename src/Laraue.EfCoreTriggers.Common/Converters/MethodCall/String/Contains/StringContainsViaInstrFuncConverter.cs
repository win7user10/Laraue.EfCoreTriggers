namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Contains
{
    public class StringContainsViaInstrFuncConverter : BaseStringContainsConverter
    {
        protected override string CombineSql(string expressionToSearchSql, string expressionToFindSql)
        {
            return $"INSTR({expressionToSearchSql}, {expressionToFindSql}) > 0";
        }
    }
}