using System.Linq;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors;
using Microsoft.EntityFrameworkCore.Metadata;
using ITrigger = Laraue.EfCoreTriggers.Common.TriggerBuilders.Abstractions.ITrigger;

namespace Laraue.EfCoreTriggers.MySql;

/// <inheritdoc />
public class MySqlTriggerVisitor : BaseTriggerVisitor
{
    private readonly ITriggerActionVisitorFactory _factory;
    private readonly ISqlGenerator _sqlGenerator;

    /// <inheritdoc />
    public MySqlTriggerVisitor(ITriggerActionVisitorFactory factory, ISqlGenerator sqlGenerator)
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

    public override string GenerateDeleteTriggerSql(string triggerName, IEntityType entityType)
    {
        return SqlBuilder.FromString($"DROP TRIGGER {triggerName};");
    }
}