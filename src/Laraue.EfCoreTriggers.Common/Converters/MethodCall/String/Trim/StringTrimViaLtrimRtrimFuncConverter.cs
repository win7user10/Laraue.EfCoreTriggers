namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Trim
{
    public class StringTrimViaLtrimRtrimFuncConverter : BaseStringTrimConverter
    {
        protected override string[] SqlTrimFunctionsNamesToApply { get; } = { "LTRIM", "RTRIM" };
    }
}