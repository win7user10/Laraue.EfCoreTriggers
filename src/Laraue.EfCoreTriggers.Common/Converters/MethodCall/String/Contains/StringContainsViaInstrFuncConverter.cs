namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Contains
{
    public class StringContainsViaInstrFuncConverter : BaseStringContainsConverter
    {
        public override string SqlFunctionName => "INSTR";
    }
}