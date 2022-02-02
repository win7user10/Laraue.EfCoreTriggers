using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.SqlGeneration
{
    public record Row
    {
        public int Ident { get; set; }
        
        public StringBuilder StringBuilder { get; set; }
    };
    
    public class SqlBuilder
    {
        private const string NewLine = "\r\n";

        public const string Ident = "  ";

        public int CurrentIdent;

        public List<Row> Rows { get; } = new ();
        
        public Row CurrentRow { get; set; }

        private SqlBuilder(Row row)
        {
            CurrentRow = row;
            Rows.Add(CurrentRow);
        }

        public SqlBuilder()
        {
            StartNewRow();
        }
        
        public static SqlBuilder FromString(string sql)
        {
            var row = new Row
            {
                StringBuilder = new StringBuilder(sql)
            };
            
            var sqlBuilder = new SqlBuilder(row);
            
            return sqlBuilder;
        }

        private SqlBuilder StartNewRow(string value = null)
        {
            var row = new Row
            {
                Ident = CurrentIdent,
                StringBuilder = new StringBuilder(value)
            };
            
            Rows.Add(row);

            CurrentRow = row;

            return this;
        }

        public SqlBuilder WithIdent(Action<SqlBuilder> action)
        {
            CurrentIdent++;
            
            action(this);

            CurrentIdent--;

            return this;
        }

        private void ExecuteForAllBesidesLast<T>(
            IEnumerable<T> values, 
            Action<T> actionForAll,
            Action<T> actionForFirst)
        {
            var valuesArray = values.ToArray();
            
            for (var i = 0; i < valuesArray.Length; i++)
            {
                actionForAll(valuesArray[i]);

                if (i != valuesArray.Length - 1)
                {
                    actionForFirst(valuesArray[i]);
                }
            }
        }

        public SqlBuilder AppendViaNewLine(IEnumerable<SqlBuilder> values)
        {
            return AppendJoin(NewLine, values);
        }

        public SqlBuilder AppendJoin(string separator, IEnumerable<string> values)
        {
            CurrentRow.StringBuilder.AppendJoin(separator, values);
            
            return this;
        }
        
        public SqlBuilder AppendJoin(string separator, IEnumerable<StringBuilder> values)
        {
            CurrentRow.StringBuilder.AppendJoin(separator, values);
            return this;
        }
        
        public SqlBuilder AppendJoin(string separator, IEnumerable<SqlBuilder> values)
        {
            ExecuteForAllBesidesLast(values, builder =>
            {
                foreach (var row in builder.Rows)
                {
                    Rows.Add(new Row()
                    {
                        Ident = row.Ident + CurrentIdent,
                        StringBuilder = row.StringBuilder
                    });
                }
            }, builder =>
            {
                if (separator is not NewLine)
                {
                    builder.CurrentRow.StringBuilder.Append(separator);
                }
            });

            return this;
        }

        public SqlBuilder Append(string value)
        {
            CurrentRow.StringBuilder.Append(value);
            return this;
        }
        
        public SqlBuilder Append(SqlBuilder sqlBuilder)
        {
            ExecuteForAllBesidesLast(sqlBuilder.Rows, row =>
            {
                CurrentRow.StringBuilder.Append(row.StringBuilder);
            }, _ => StartNewRow());
            
            return this;
        }
        
        public SqlBuilder Prepend(string value)
        {
            CurrentRow.StringBuilder.Insert(0, value);
            return this;
        }

        public SqlBuilder AppendNewLine(string value)
        {
            return StartNewRow(value);
        }

        private string GetIdent(int ident)
        {
            return string.Concat(Enumerable.Range(0, ident).Select(_ => Ident));
        }

        public SqlBuilder Append(char value)
        {
            CurrentRow.StringBuilder.Append(value);
            return this;
        }

        public static implicit operator string(SqlBuilder @this) => @this.ToString();

        public override string ToString()
        {
            var fullSql = new StringBuilder();

            ExecuteForAllBesidesLast(Rows, row =>
            {
                var ident = GetIdent(row.Ident);
                
                fullSql.Append(ident)
                    .Append(row.StringBuilder);
            }, _ => fullSql.Append(NewLine));

            return fullSql.ToString();
        }
    }
}
