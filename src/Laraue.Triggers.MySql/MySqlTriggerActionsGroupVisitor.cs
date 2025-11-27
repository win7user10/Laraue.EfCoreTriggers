using System.Linq;
using Laraue.Triggers.Core.SqlGeneration;
using Laraue.Triggers.Core.Visitors.TriggerVisitors;

namespace Laraue.Triggers.MySql;

/// <inheritdoc />
public sealed class MySqlTriggerActionsGroupVisitor : BaseTriggerActionsGroupVisitor
{
    /// <inheritdoc />
    public MySqlTriggerActionsGroupVisitor(ITriggerActionVisitorFactory factory)
        : base(factory)
    {
    }
    
    /// <inheritdoc />
    protected override SqlBuilder GetActionSql(SqlBuilder[] actionsSql, SqlBuilder[] conditionsSql)
    {
        var sql = new SqlBuilder();
        var isAnyCondition = conditionsSql.Length > 0;
        
        if (isAnyCondition)
        {
            sql.AppendNewLine($"IF ")
                .AppendJoin(" AND ", conditionsSql.Select(x => x.ToString()))
                .Append(" THEN ");
        }

        sql.WithIdentWhen(conditionsSql.Length > 0,  loopSql => loopSql.AppendViaNewLine(actionsSql));
        
        if (isAnyCondition)
        {
            sql.AppendNewLine($"END IF;");
        }

        return sql;
    }
}