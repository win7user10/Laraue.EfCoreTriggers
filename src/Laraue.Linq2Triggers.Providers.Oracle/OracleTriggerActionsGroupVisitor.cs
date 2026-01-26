using System.Linq;
using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.Visitors.TriggerVisitors;

namespace Laraue.Linq2Triggers.Providers.Oracle;

/// <inheritdoc />
public sealed class OracleTriggerActionsGroupVisitor : BaseTriggerActionsGroupVisitor
{
    /// <inheritdoc />
    public OracleTriggerActionsGroupVisitor(ITriggerActionVisitorFactory factory)
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
            sql.AppendNewLine("IF ")
                .AppendJoin(" AND ", conditionsSql.Select(x => x.ToString()))
                .Append(" THEN ");
        }

        sql.WithIdentWhen(conditionsSql.Length > 0,  loopSql => loopSql.AppendViaNewLine(actionsSql));
        
        if (isAnyCondition)
        {
            sql.AppendNewLine("END IF;");
        }

        return sql;
    }
}