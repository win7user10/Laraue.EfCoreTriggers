using Laraue.Core.Extensions;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Laraue.EfCoreTriggers.Common.Builders.Visitor
{
    public class SqlExtendedResult
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public string Sql => _stringBuilder.ToString();

        public readonly HashSet<MemberInfo> AffectedColumns = new HashSet<MemberInfo>();

        public SqlExtendedResult(IEnumerable<MemberInfo> affectedColumnsInfo)
            => affectedColumnsInfo.SafeForEach(x => AffectedColumns.Add(x));

        public SqlExtendedResult(IEnumerable<SqlExtendedResult> subResults)
            => subResults.SafeForEach(subResult => subResult.AffectedColumns.SafeForEach(member => AffectedColumns.Add(member)));

        public SqlExtendedResult(IEnumerable<SqlExtendedResult> subResults, string sql)
            : this(subResults) => Append(sql);

        public SqlExtendedResult(SqlExtendedResult subResult, string sql)
            : this(new[] { subResult }, sql)
        {
        }

        public SqlExtendedResult(MemberInfo affectedColumnInfo)
            => AffectedColumns.Add(affectedColumnInfo);

        public SqlExtendedResult(string sql)
            => Append(sql);

        public void MergeColumnsInfo(IEnumerable<SqlExtendedResult> subResults)
            => subResults.SafeForEach(subResult => subResult.AffectedColumns.SafeForEach(member => AffectedColumns.Add(member)));

        public void MergeColumnsInfo(SqlExtendedResult subResult)
            => subResult.AffectedColumns.SafeForEach(member => AffectedColumns.Add(member));

        public SqlExtendedResult Append(string value)
        {
            _stringBuilder.Append(value);
            return this;
        }

        public SqlExtendedResult Append(char value)
        {
            _stringBuilder.Append(value);
            return this;
        }

        public static implicit operator string(SqlExtendedResult result) => result.Sql;

        public override string ToString() => Sql;
    }
}
