namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.EndsWith
{
    public class StringEndsWithViaPlusConverter : BaseStringEndsWithConverter
    {
        public override string BuildEndSql(string argumentSql)
        {
            return $"('%' + {argumentSql})";
        }
    }
}