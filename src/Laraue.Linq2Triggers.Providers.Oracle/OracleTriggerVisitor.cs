using System.Linq;
using Laraue.Linq2Triggers.Core;
using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.TriggerBuilders.Abstractions;
using Laraue.Linq2Triggers.Core.Visitors.TriggerVisitors;

namespace Laraue.Linq2Triggers.Providers.Oracle;

/// <inheritdoc />
public class OracleTriggerVisitor : BaseTriggerVisitor
{
    private readonly ITriggerActionVisitorFactory _factory;
    private readonly ISqlGenerator _sqlGenerator;

    /// <inheritdoc />
    public OracleTriggerVisitor(ITriggerActionVisitorFactory factory, ISqlGenerator sqlGenerator)
    {
        _factory = factory;
        _sqlGenerator = sqlGenerator;
    }

    /// <inheritdoc />
    public override string GenerateCreateTriggerSql(ITrigger trigger)
    {
        var triggerTimeName = GetTriggerTimeName(trigger.TriggerTime);
        
        var actionsSql = trigger.Actions
            .Select(action => _factory.Visit(action, new VisitedMembers()))
            .ToArray();
        
        var sql = SqlBuilder.FromString($"CREATE TRIGGER {trigger.Name}")
            .AppendNewLine($"{triggerTimeName} {trigger.TriggerEvent.ToString().ToUpper()} ON {_sqlGenerator.GetTableSql(trigger.TriggerEntityType)}")
            .AppendNewLine("FOR EACH ROW")
            .AppendNewLine("BEGIN")
            .WithIdent(triggerSql => triggerSql.AppendViaNewLine(actionsSql))
            .AppendNewLine("END");
        
        return sql;
    }

    public override string GenerateDeleteTriggerSql(string triggerName, ITriggerEntityType entityType)
    {
        return SqlBuilder.FromString($"DROP TRIGGER {triggerName};");
    }
}