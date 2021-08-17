namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Contains
{
    public class StringContainsViaStrposFuncConverter : BaseStringContainsConverter
    {
        public override string SqlFunctionName => "STRPOS";
    }
}