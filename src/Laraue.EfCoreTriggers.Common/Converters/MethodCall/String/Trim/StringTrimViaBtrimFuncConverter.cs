namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Trim
{
    public class StringTrimViaBtrimFuncConverter : BaseStringTrimConverter
    {
        protected override string[] SqlTrimFunctionsNamesToApply { get; } = { "BTRIM" };
    }
}