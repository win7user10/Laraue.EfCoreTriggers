using System.Linq;
using Laraue.EfCoreTriggers.SqlGeneration;

namespace Laraue.EfCoreTriggers.Converters.MethodCall.String.Concat
{
    public class ConcatStringViaDoubleVerticalLineConverter : BaseStringConcatConverter
    {
        /// <inheritdoc />
        public override SqlBuilder BuildSql(SqlBuilder[] argumentsSql)
        {

            return new SqlBuilder(argumentsSql)
                .AppendJoin(" || ", argumentsSql.Select(x => x.StringBuilder));
        }
    }
}