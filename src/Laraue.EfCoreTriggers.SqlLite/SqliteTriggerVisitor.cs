﻿using System.Linq;
using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.SqlLite;

public class SqliteTriggerVisitor : BaseTriggerVisitor
{
    private readonly ITriggerActionVisitorFactory _factory;
    private readonly IDbSchemaRetriever _dbSchemaRetriever;

    public SqliteTriggerVisitor(ITriggerActionVisitorFactory factory, IDbSchemaRetriever dbSchemaRetriever)
    {
        _factory = factory;
        _dbSchemaRetriever = dbSchemaRetriever;
    }

    public override string GenerateCreateTriggerSql(ITrigger trigger)
    {
        var conditionSql = new SqlBuilder();
        var sql = new SqlBuilder();
        
        if (trigger.Conditions.Count > 0)
        {
            var conditionsSql = trigger.Conditions
                .Select(actionCondition => _factory.Visit(actionCondition, new VisitedMembers()));

            conditionSql
                .Append("WHEN ")
                .WithIdent(x => x
                    .AppendNewLine()
                    .AppendJoin(" AND ", conditionsSql.Select(x => x.ToString())));
        }
        
        var actionsSql = trigger.Actions
            .Select(action => _factory.Visit(action, new VisitedMembers()))
            .ToArray();
        
        var actionsCount = actionsSql.Length;
        var triggerTimeName = GetTriggerTimeName(trigger.TriggerTime);
        
        // Reverse trigger actions to fire it in the order set while trigger configuring
        for (var i = actionsCount; i > 0; i--)
        {
            var postfix = actionsCount > 1 ? $"_{actionsCount - i}" : string.Empty;
            var action = actionsSql[i - 1];

            var tableName = _dbSchemaRetriever.GetTableName(trigger.TriggerEntityType);
            
            sql.Append($"CREATE TRIGGER {trigger.Name}{postfix}")
                .AppendNewLine($"{triggerTimeName} {trigger.TriggerEvent.ToString().ToUpper()} ON {tableName}")
                .AppendNewLine("FOR EACH ROW")
                .AppendNewLine(conditionSql)
                .AppendNewLine("BEGIN")
                .WithIdent(x => x.AppendNewLine(action))
                .AppendNewLine("END;");
        }
        
        return sql;
    }

    public override string GenerateDeleteTriggerSql(string triggerName)
    {
        return SqlBuilder.FromString("PRAGMA writable_schema = 1; ")
            .AppendNewLine($"DELETE FROM sqlite_master WHERE type = 'trigger' AND name like '{triggerName}%';")
            .AppendNewLine("PRAGMA writable_schema = 0;");
    }
}