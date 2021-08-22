namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.EndsWith
{
    public class StringEndsWithViaDoubleVerticalLineConverter : BaseStringEndsWithConverter
    {
        public override string BuildEndSql(string argumentSql)
        {
            return $"('%' || {argumentSql})";
        }
    }
}