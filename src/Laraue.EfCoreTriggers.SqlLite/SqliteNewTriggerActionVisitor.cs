using System.Linq;
using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.SqlLite;

public sealed class SqliteNewTriggerActionVisitor : ITriggerActionVisitor<NewTriggerAction>
{
    private readonly ITriggerActionVisitorFactory _factory;

    public SqliteNewTriggerActionVisitor(ITriggerActionVisitorFactory factory)
    {
        _factory = factory;
    }

    public SqlBuilder Visit(NewTriggerAction triggerAction, VisitedMembers visitedMembers)
    {
        var conditionSql = new SqlBuilder();
        var sql = new SqlBuilder();
        
        var actionsSql = triggerAction.ActionExpressions
            .Select(action => _factory.Visit(action, new VisitedMembers()))
            .ToArray();
        
        if (triggerAction.ActionConditions.Count > 0)
        {
            var conditionsSql = triggerAction.ActionConditions
                .Select(actionCondition => _factory.Visit(actionCondition, new VisitedMembers()));

            conditionSql
                .Append("WHEN ")
                .WithIdent(x => x
                    .AppendNewLine()
                    .AppendJoin(" AND ", conditionsSql.Select(x => x.ToString())));
        }

        sql.AppendNewLine("FOR EACH ROW")
            .AppendNewLine(conditionSql)
            .AppendNewLine("BEGIN")
            .WithIdent(x => x.AppendViaNewLine(actionsSql));

        return sql;
    }
}