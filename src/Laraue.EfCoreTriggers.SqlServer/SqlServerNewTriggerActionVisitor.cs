using System.Linq;
using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.SqlServer;

public sealed class SqlServerNewTriggerActionVisitor : BaseNewTriggerActionVisitor
{
    public SqlServerNewTriggerActionVisitor(ITriggerActionVisitorFactory factory)
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