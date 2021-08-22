namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.EndsWith
{
    public class StringEndsWithViaConcatFuncConverter : BaseStringEndsWithConverter
    {
        public override string BuildEndSql(string argumentSql)
        {
            return $"CONCAT('%', {argumentSql})";
        }
    }
}