using System.Linq;
using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.SqlLite;

public sealed class SqliteNewTriggerActionVisitor : BaseNewTriggerActionVisitor
{
    public SqliteNewTriggerActionVisitor(ITriggerActionVisitorFactory factory)
        : base(factory)
    {
    }
    
    protected override SqlBuilder GetActionSql(SqlBuilder[] actionsSql, SqlBuilder[] conditionsSql)
    {
        var sql = new SqlBuilder()
            .AppendNewLine("FOR EACH ROW");
        
        if (conditionsSql.Length > 0)
        {
            sql
                .AppendNewLine("WHEN ")
                .WithIdent(x => x
                    .AppendNewLine()
                    .AppendJoin(" AND ", conditionsSql.Select(y => y.ToString())));
        }

        sql.AppendNewLine("BEGIN")
            .WithIdent(x => x.AppendViaNewLine(actionsSql));

        return sql;
    }
}