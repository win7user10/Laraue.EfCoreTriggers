using System.Linq;
using Laraue.Linq2Triggers.Core;
using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.TriggerBuilders.Abstractions;
using Laraue.Linq2Triggers.Core.Visitors.TriggerVisitors;

namespace Laraue.Linq2Triggers.Providers.Sqlite;

public class SqliteTriggerVisitor : BaseTriggerVisitor
{
    private readonly ITriggerActionVisitorFactory _factory;
    private readonly ISqlGenerator _sqlGenerator;

    public SqliteTriggerVisitor(ITriggerActionVisitorFactory factory, ISqlGenerator sqlGenerator)
    {
        _factory = factory;
        _sqlGenerator = sqlGenerator;
    }

    public override string GenerateCreateTriggerSql(ITrigger trigger)
    {
        var sql = new SqlBuilder();
        
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

            var tableName = _sqlGenerator.GetTableSql(trigger.TriggerEntityType);
            
            sql.Append($"CREATE TRIGGER {trigger.Name}{postfix}")
                .AppendNewLine($"{triggerTimeName} {trigger.TriggerEvent.ToString().ToUpper()} ON {tableName}")
                .Append(action)
                .AppendNewLine("END;");
        }
        
        return sql;
    }

    public override string GenerateDeleteTriggerSql(string triggerName, ITriggerEntityType entityType)
    {
        return SqlBuilder.FromString("PRAGMA writable_schema = 1; ")
            .AppendNewLine($"DELETE FROM sqlite_master WHERE type = 'trigger' AND name like '{triggerName}%';")
            .AppendNewLine("PRAGMA writable_schema = 0;");
    }
}