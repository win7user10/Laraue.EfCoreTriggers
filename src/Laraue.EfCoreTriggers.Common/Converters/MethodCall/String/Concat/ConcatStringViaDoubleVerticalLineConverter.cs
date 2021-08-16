using System.Linq;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Concat
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