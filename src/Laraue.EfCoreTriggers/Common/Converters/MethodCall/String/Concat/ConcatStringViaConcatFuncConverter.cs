using System.Linq;
using Laraue.EfCoreTriggers.Common.Builders.Providers;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Concat
{
    public class ConcatStringViaConcatFuncConverter : BaseStringConcatConverter
    {
        /// <inheritdoc />
        public override SqlBuilder BuildSql(SqlBuilder[] argumentsSql)
        {
            return new SqlBuilder(argumentsSql)
                .Append("CONCAT(")
                .AppendJoin(", ", argumentsSql.Select(x => x.StringBuilder))
                .Append(")");
        }
    }
}