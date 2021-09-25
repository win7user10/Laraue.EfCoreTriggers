using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.SqlGeneration
{
    public class SqlBuilder
    {
        public string Sql => StringBuilder.ToString();

        public StringBuilder StringBuilder { get; } = new();

        public const string NewLine = "\r\n";

        /// <summary>
        /// Contains info about all members taking part in generated SQL.
        /// </summary>
        public readonly Dictionary<ArgumentType, HashSet<MemberInfo>> AffectedColumns
            = new()
            {
                [ArgumentType.New] = new HashSet<MemberInfo>(new MemberInfoComparer()),
                [ArgumentType.Old] = new HashSet<MemberInfo>(new MemberInfoComparer()),
            };

        private class MemberInfoComparer : IEqualityComparer<MemberInfo>
        {
            public bool Equals(MemberInfo x, MemberInfo y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return Equals(x.DeclaringType, y.DeclaringType) && x.Name == y.Name;
            }

            public int GetHashCode(MemberInfo obj)
            {
                return HashCode.Combine(obj.DeclaringType, obj.Name);
            }
        }

        /// <summary>
        /// Create instance of <see cref="SqlBuilder"/>, merging AffectedColumns from passed builders.
        /// </summary>
        /// <param name="generatedSqls"></param>
        public SqlBuilder(IEnumerable<SqlBuilder> generatedSqls)
            => MergeColumnsInfo(generatedSqls);

        public SqlBuilder(Dictionary<ArgumentType, HashSet<MemberInfo>> affectedColumns)
            => MergeColumnsInfo(affectedColumns);

        public SqlBuilder(Dictionary<ArgumentType, HashSet<MemberInfo>> affectedColumns, string sql)
            : this(affectedColumns) => Append(sql);

        public SqlBuilder(MemberInfo affectedColumn, ArgumentType argumentType)
            => MergeColumnInfo(affectedColumn, argumentType);

        public SqlBuilder(string sql)
            => Append(sql);

        public SqlBuilder()
        {
        }

        public SqlBuilder MergeColumnsInfo(IEnumerable<SqlBuilder> generatedSqls)
        {
            foreach (var generatedSql in generatedSqls)
                MergeColumnsInfo(generatedSql.AffectedColumns);
            return this;
        }

        public SqlBuilder MergeColumnsInfo(SqlBuilder generatedSql)
            => MergeColumnsInfo(new[] { generatedSql });

        public SqlBuilder MergeColumnsInfo(Dictionary<ArgumentType, HashSet<MemberInfo>> affectedColumns)
        {
            foreach (var affectedColumn in affectedColumns)
                AffectedColumns[affectedColumn.Key].AddRange(affectedColumn.Value);
            return this;
        }

        public SqlBuilder MergeColumnInfo(MemberInfo affectedColumn, ArgumentType argumentType)
        {
            switch (argumentType)
            {
                case ArgumentType.New:
                case ArgumentType.Old:
                    AffectedColumns[argumentType].Add(affectedColumn);
                    break;
            }
            return this;
        }

        public SqlBuilder Append(StringBuilder builder)
        {
            StringBuilder.Append(builder);
            return this;
        }

        public SqlBuilder AppendJoin(IEnumerable<StringBuilder> builders)
            => AppendJoin(string.Empty, builders);

        public SqlBuilder AppendJoin(string separator, IEnumerable<string> values)
        {
            StringBuilder.AppendJoin(separator, values);
            return this;
        }

        public SqlBuilder AppendJoin(string separator, IEnumerable<StringBuilder> builders)
        {
            var buildersArray = builders.ToArray();
            for (int i = 0; i < buildersArray.Length; i++)
            {
                StringBuilder.Append(buildersArray[i]);
                if (i < buildersArray.Length - 1)
                    StringBuilder.Append(separator);
            }
            return this;
        }

        public SqlBuilder Append(string value)
        {
            StringBuilder.Append(value);
            return this;
        }
        
        public SqlBuilder Prepend(string value)
        {
            StringBuilder.Insert(0, value);
            return this;
        }

        public SqlBuilder AppendNewLine(string value)
            => Append($"{NewLine}{value}");

        public SqlBuilder Append(char value)
        {
            StringBuilder.Append(value);
            return this;
        }

        public static implicit operator string(SqlBuilder @this) => @this.Sql;

        public override string ToString() => Sql;
    }

    internal static class HashSetExtensions
    {
        public static HashSet<T> AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> values)
        {
            foreach (var value in values)
                hashSet.Add(value);
            return hashSet;
        }
    }
}
