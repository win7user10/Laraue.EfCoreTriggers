using Laraue.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Laraue.EfCoreTriggers.Common.Builders.Visitor
{
    public class GeneratedSql
    {
        public string Sql => SqlBuilder.ToString();

        public StringBuilder SqlBuilder { get; } = new StringBuilder();

        public readonly HashSet<MemberInfo> AffectedColumns = new HashSet<MemberInfo>();

        public GeneratedSql(IEnumerable<MemberInfo> affectedColumnsInfo)
            => affectedColumnsInfo.SafeForEach(x => AffectedColumns.Add(x));

        public GeneratedSql(IEnumerable<MemberInfo> affectedColumnsInfo, string sql)
            : this(affectedColumnsInfo) => Append(sql);

        public GeneratedSql(IEnumerable<GeneratedSql> subResults)
            => subResults.SafeForEach(subResult => subResult.AffectedColumns.SafeForEach(member => AffectedColumns.Add(member)));

        public GeneratedSql(IEnumerable<GeneratedSql> subResults, string sql)
            : this(subResults) => Append(sql);

        public GeneratedSql(MemberInfo affectedColumnInfo)
            => AffectedColumns.Add(affectedColumnInfo);

        public GeneratedSql(string sql)
            => Append(sql);

        public GeneratedSql()
        {
        }

        public GeneratedSql MergeColumnsInfo(IEnumerable<MemberInfo> subResults)
        {
            subResults.SafeForEach(x => AffectedColumns.Add(x));
            return this;
        }

        public void MergeColumnsInfo(GeneratedSql subResult)
            => subResult.AffectedColumns.SafeForEach(member => AffectedColumns.Add(member));

        public GeneratedSql Append(StringBuilder builder)
        {
            SqlBuilder.Append(builder);
            return this;
        }

        public GeneratedSql AppendJoin(IEnumerable<StringBuilder> builders)
            => AppendJoin(string.Empty, builders);

        public GeneratedSql AppendJoin(string separator, IEnumerable<StringBuilder> builders)
        {
            var lastBuilder = builders.Last();
            builders.SafeForEach(builder =>
            {
                SqlBuilder.Append(builder);
                if (builder != lastBuilder)
                    SqlBuilder.Append(separator);
            });
            return this;
        }

        public GeneratedSql Append(string value)
        {
            SqlBuilder.Append(value);
            return this;
        }

        public GeneratedSql Append(char value)
        {
            SqlBuilder.Append(value);
            return this;
        }

        public static implicit operator string(GeneratedSql @this) => @this.Sql;

        public override string ToString() => Sql;
    }
}
