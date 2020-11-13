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

        public readonly Dictionary<ArgumentPrefix, HashSet<MemberInfo>> AffectedColumns
            = new Dictionary<ArgumentPrefix, HashSet<MemberInfo>>
            {
                [ArgumentPrefix.New] = new HashSet<MemberInfo>(),
                [ArgumentPrefix.Old] = new HashSet<MemberInfo>(),
            };


        public GeneratedSql(IEnumerable<GeneratedSql> generatedSqls)
            => MergeColumnsInfo(generatedSqls);

        public GeneratedSql(Dictionary<ArgumentPrefix, HashSet<MemberInfo>> affectedColumns)
            => MergeColumnsInfo(affectedColumns);

        public GeneratedSql(Dictionary<ArgumentPrefix, HashSet<MemberInfo>> affectedColumns, string sql)
            : this(affectedColumns) => Append(sql);

        public GeneratedSql(MemberInfo affectedColumn, ArgumentPrefix argumentPrefix)
            => MergeColumnInfo(affectedColumn, argumentPrefix);

        public GeneratedSql(string sql)
            => Append(sql);

        public GeneratedSql()
        {
        }

        public GeneratedSql MergeColumnsInfo(IEnumerable<GeneratedSql> generatedSqls)
        {
            generatedSqls.SafeForEach(x => MergeColumnsInfo(x.AffectedColumns));
            return this;
        }

        public GeneratedSql MergeColumnsInfo(Dictionary<ArgumentPrefix, HashSet<MemberInfo>> affectedColumns)
        {
            affectedColumns.SafeForEach(x => AffectedColumns[x.Key].AddRange(x.Value));
            return this;
        }

        public GeneratedSql MergeColumnInfo(MemberInfo affectedColumn, ArgumentPrefix argumentPrefix)
        {
            if (argumentPrefix == ArgumentPrefix.New || argumentPrefix == ArgumentPrefix.Old)
                AffectedColumns[argumentPrefix].Add(affectedColumn);
            return this;
        }

        public GeneratedSql Append(GeneratedSql generatedSql)
            => Append(generatedSql.SqlBuilder);

        public GeneratedSql Append(StringBuilder builder)
        {
            SqlBuilder.Append(builder);
            return this;
        }

        public GeneratedSql AppendJoin(IEnumerable<StringBuilder> builders)
            => AppendJoin(string.Empty, builders);

        public GeneratedSql AppendJoin(string separator, IEnumerable<string> values)
        {
            SqlBuilder.AppendJoin(separator, values);
            return this;
        }

        public GeneratedSql AppendJoin(string separator, IEnumerable<StringBuilder> builders)
        {
            var buildersArray = builders.ToArray();
            for (int i = 0; i < buildersArray.Length; i++)
            {
                SqlBuilder.Append(buildersArray[i]);
                if (i < buildersArray.Length - 1)
                    SqlBuilder.Append(separator);
            }
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

    internal static class HashSetExtensions
    {
        public static HashSet<T> AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> values)
        {
            values.SafeForEach(x => hashSet.Add(x));
            return hashSet;
        }
    }
}
