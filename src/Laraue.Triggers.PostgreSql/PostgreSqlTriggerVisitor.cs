using System.Linq;
using Laraue.Triggers.Core;
using Laraue.Triggers.Core.SqlGeneration;
using Laraue.Triggers.Core.TriggerBuilders;
using Laraue.Triggers.Core.TriggerBuilders.Abstractions;
using Laraue.Triggers.Core.Visitors.TriggerVisitors;

namespace Laraue.Triggers.PostgreSql;

public class PostgreSqlTriggerVisitor : BaseTriggerVisitor
{
    private readonly ITriggerActionVisitorFactory _factory;
    private readonly ISqlGenerator _sqlGenerator;

    public PostgreSqlTriggerVisitor(ITriggerActionVisitorFactory factory, ISqlGenerator sqlGenerator)
    {
        _factory = factory;
        _sqlGenerator = sqlGenerator;
    }

    public override string GenerateCreateTriggerSql(ITrigger trigger)
    {
        var actionsSql = trigger.Actions
            .Select(action => _factory.Visit(action, new VisitedMembers()))
            .ToArray();

        var functionName = _sqlGenerator.GetFunctionNameSql(trigger.TriggerEntityType, trigger.Name);
        
        var sql = SqlBuilder.FromString($"CREATE FUNCTION {functionName}() RETURNS trigger as ${trigger.Name}$")
            .AppendNewLine("BEGIN")
            .WithIdent(triggerSql => triggerSql.AppendViaNewLine(actionsSql));
        
            var tableRef = trigger.TriggerEvent == TriggerEvent.Delete ? "OLD" : "NEW";
            
            sql.AppendNewLine($"RETURN {tableRef};")
                .AppendNewLine("END;")
                .AppendNewLine($"${trigger.Name}$ LANGUAGE plpgsql;")
                .AppendNewLine($"CREATE TRIGGER {trigger.Name} {GetTriggerTimeName(trigger.TriggerTime)} {trigger.TriggerEvent.ToString().ToUpper()}")
                .AppendNewLine($"ON {_sqlGenerator.GetTableSql(trigger.TriggerEntityType)}")
                .AppendNewLine($"FOR EACH ROW EXECUTE PROCEDURE {functionName}();");
        
        return sql;
    }

    public override string GenerateDeleteTriggerSql(string triggerName, ITriggerEntityType entityType)
    {
        var functionName = _sqlGenerator.GetFunctionNameSql(entityType, triggerName);
        
        return SqlBuilder.FromString($"DROP FUNCTION {functionName}() CASCADE;");
    }
}