using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laraue.EfCoreTriggers.Common.SqlGeneration
{
    /// <summary>
    /// Class for SQL code generating.
    /// </summary>
    public class SqlBuilder
    {
        private const string NewLine = "\r\n";

        /// <summary>
        /// String used for ident in the builder.
        /// </summary>
        private const string Ident = "  ";

        /// <summary>
        /// Current ident of SQL builder. All new rows will inherit this ident.
        /// </summary>
        private int CurrentIdent { get; set; }

        /// <summary>
        /// All rows in <see cref="SqlBuilder"/>.
        /// </summary>
        private List<SqlBuilderRow> Rows { get; } = new ();

        /// <summary>
        /// Latest row of <see cref="SqlBuilder"/>.
        /// </summary>
        private SqlBuilderRow CurrentSqlBuilderRow => Rows.Last();

        private SqlBuilder(SqlBuilderRow sqlBuilderRow)
        {
            Rows.Add(sqlBuilderRow);
        }

        /// <summary>
        /// Initialize new <see cref="SqlBuilder"/> with empty content.
        /// </summary>
        public SqlBuilder()
        {
            StartNewRow();
        }
        
        /// <summary>
        /// Initialize new <see cref="SqlBuilder"/> with passed content.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static SqlBuilder FromString(string sql)
        {
            var row = new SqlBuilderRow(0, sql);
            
            var sqlBuilder = new SqlBuilder(row);
            
            return sqlBuilder;
        }

        private SqlBuilder StartNewRow(string? value = null)
        {
            return StartNewRow(new SqlBuilderRow(CurrentIdent, value));
        }
        
        private SqlBuilder StartNewRow(SqlBuilderRow sqlBuilderRow)
        {
            Rows.Add(sqlBuilderRow);

            return this;
        }

        /// <summary>
        /// Returns to the delegate instance of
        /// <see cref="SqlBuilder"/> with increased ident.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public SqlBuilder WithIdent(Action<SqlBuilder> action)
        {
            CurrentIdent++;

            StartNewRow();
            
            action(this);

            CurrentIdent--;

            return this;
        }

        /// <summary>
        /// Add ident if predicate is passed and executes action on the <see cref="SqlBuilder"/>.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public SqlBuilder WithIdentWhen(bool predicate, Action<SqlBuilder> action)
        {
            if (predicate)
            {
                return WithIdent(action);
            }
            
            action(this);
            return this;

        }

        private void ExecuteForAllBesidesLast<T>(
            IEnumerable<T> values, 
            Action<T, int> actionForAll,
            Action<T, int> actionForFirst)
        {
            var valuesArray = values.ToArray();
            
            for (var i = 0; i < valuesArray.Length; i++)
            {
                actionForAll(valuesArray[i], i);

                if (i != valuesArray.Length - 1)
                {
                    actionForFirst(valuesArray[i], i);
                }
            }
        }

        /// <summary>
        /// Append passed sequence of <see cref="SqlBuilder"/> with rule:
        /// The first line of first builder appends to latest row.
        /// Other lines appends as new rows with
        /// <see cref="SqlBuilderRow.Ident"/> = <see cref="SqlBuilderRow.Ident"/> + <see cref="CurrentIdent"/>.
        /// </summary>
        /// <param name="values">Sql builders to append</param>
        /// <returns></returns>
        public SqlBuilder AppendViaNewLine(IEnumerable<SqlBuilder> values)
        {
            return AppendJoin(NewLine, values);
        }
        
        /// <summary>
        /// Append to each sql builder passed separator and join them via new line.
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public SqlBuilder AppendViaNewLine(string separator, IEnumerable<SqlBuilder> values)
        {
            var arrayValues = values.ToArray();
            
            ExecuteForAllBesidesLast(arrayValues, (_, _) => { }, (x, _) => x.Append(separator));

            return AppendViaNewLine(arrayValues);
        }

        /// <summary>
        /// Append sequence of strings to row of current SQL builder.
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public SqlBuilder AppendJoin(string separator, IEnumerable<string> values)
        {
            CurrentSqlBuilderRow.StringBuilder.AppendJoin(separator, values);
            
            return this;
        }
        
        /// <summary>
        /// Append sequence of <see cref="SqlBuilder"/> via passed separator.
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public SqlBuilder AppendJoin(string separator, IEnumerable<SqlBuilder> values)
        {
            ExecuteForAllBesidesLast(values, (builder, _) =>
            {
                Append(builder);
            }, (_, _) =>
            {
                if (separator is not NewLine)
                {
                    CurrentSqlBuilderRow.StringBuilder.Append(separator);
                }
                else
                {
                    StartNewRow(new SqlBuilderRow(CurrentSqlBuilderRow.Ident));
                }
            });

            return this;
        }

        /// <summary>
        /// Append string value to current <see cref="SqlBuilder"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilder Append(string value)
        {
            CurrentSqlBuilderRow.StringBuilder.Append(value);
            return this;
        }
        
        /// <summary>
        /// Append rows of passed <see cref="SqlBuilder"/> to rows of current <see cref="SqlBuilder"/>.
        /// </summary>
        /// <param name="sqlBuilder"></param>
        /// <returns></returns>
        public SqlBuilder Append(SqlBuilder sqlBuilder)
        {
            ExecuteForAllBesidesLast(sqlBuilder.Rows, (row, index) =>
            {
                if (index != 0)
                {
                    CurrentSqlBuilderRow.Ident = CurrentIdent + row.Ident;
                }
                    
                CurrentSqlBuilderRow.StringBuilder.Append(row.StringBuilder);

            }, (_, _) =>
            {
                StartNewRow(new SqlBuilderRow(CurrentIdent));
            });
            
            return this;
        }
        
        /// <summary>
        /// Append string to the start of row of the current <see cref="SqlBuilder"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilder Prepend(string value)
        {
            CurrentSqlBuilderRow.StringBuilder.Insert(0, value);
            return this;
        }

        /// <summary>
        /// Starts new row in <see cref="SqlBuilder"/> with passed string value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilder AppendNewLine(string? value = null)
        {
            return StartNewRow(value);
        }
        
        /// <summary>
        /// Starts new row and append passed <see cref="SqlBuilder"/>.
        /// </summary>
        /// <param name="sqlBuilder"></param>
        /// <returns></returns>
        public SqlBuilder AppendNewLine(SqlBuilder sqlBuilder)
        {
            StartNewRow();
            
            return Append(sqlBuilder);
        }

        private static string GetIdent(int ident)
        {
            return string.Concat(Enumerable.Range(0, ident).Select(_ => Ident));
        }

        /// <summary>
        /// Append char to the current row of <see cref="SqlBuilder"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilder Append(char value)
        {
            CurrentSqlBuilderRow.StringBuilder.Append(value);
            return this;
        }

        /// <summary>
        /// Get SQL code from current builder.
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static implicit operator string(SqlBuilder @this) => @this.ToString();

        /// <summary>
        /// Get the final SQL code.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var fullSql = new StringBuilder();

            ExecuteForAllBesidesLast(Rows, (row, _) =>
            {
                var ident = GetIdent(row.Ident);
                
                fullSql.Append(ident)
                    .Append(row.StringBuilder);
            }, (_, _) => fullSql.Append(NewLine));

            return fullSql.ToString();
        }
    }
}
