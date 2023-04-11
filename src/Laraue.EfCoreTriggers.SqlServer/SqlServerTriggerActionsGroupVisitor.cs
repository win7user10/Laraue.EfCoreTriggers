using System.Linq;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors;

namespace Laraue.EfCoreTriggers.SqlServer;

public sealed class SqlServerTriggerActionsGroupVisitor : BaseTriggerActionsGroupVisitor
{
    public SqlServerTriggerActionsGroupVisitor(ITriggerActionVisitorFactory factory)
        : base(factory)
    {
    }

    protected override SqlBuilder GetActionSql(SqlBuilder[] actionsSql, SqlBuilder[] conditionsSql)
    {
        var sql = new SqlBuilder();
        
        if (conditionsSql.Length > 0)
        {
            sql.Append($"IF (")
                .AppendJoin(" AND ", conditionsSql.Select(x => x.ToString()))
                .Append(")")
                .AppendNewLine();
        }
        
        return sql.AppendViaNewLine(actionsSql);
    }
}