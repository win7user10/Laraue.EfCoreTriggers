namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Contains
{
    public class StringContainsViaCharindexFuncConverter : BaseStringContainsConverter
    {
        public override string SqlFunctionName => "CHARINDEX";
    }
}