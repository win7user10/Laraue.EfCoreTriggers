namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Trim
{
    public class StringTrimViaTrimFuncConverter : BaseStringTrimConverter
    {
        protected override string[] SqlTrimFunctionsNamesToApply { get; } = { "TRIM" };
    }
}