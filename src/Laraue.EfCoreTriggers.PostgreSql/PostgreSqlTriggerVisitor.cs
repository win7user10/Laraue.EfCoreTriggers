using System.Linq;
using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ITrigger = Laraue.EfCoreTriggers.Common.TriggerBuilders.Base.ITrigger;

namespace Laraue.EfCoreTriggers.PostgreSql;

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
            .WithIdent(triggerSql =>
            {
                if (trigger.Conditions.Count > 0)
                {
                    var conditionsSql = trigger.Conditions
                        .Select(actionCondition => _factory.Visit(actionCondition, new VisitedMembers()));
            
                    triggerSql.Append("IF ")
                        .AppendJoin(" AND ", conditionsSql.Select(x => x.ToString()))
                        .Append(" THEN ");
                }

                triggerSql.WithIdentWhen(trigger.Conditions.Count > 0, loopSql => loopSql.AppendViaNewLine(actionsSql));
        
                if (trigger.Conditions.Count > 0)
                {
                    triggerSql.AppendNewLine($"END IF;");
                }
            })
            .AppendNewLine("RETURN NEW;")
            .AppendNewLine("END;")
            .AppendNewLine($"${trigger.Name}$ LANGUAGE plpgsql;")
            .AppendNewLine($"CREATE TRIGGER {trigger.Name} {GetTriggerTimeName(trigger.TriggerTime)} {trigger.TriggerEvent.ToString().ToUpper()}")
            .AppendNewLine($"ON {_sqlGenerator.GetTableSql(trigger.TriggerEntityType)}")
            .AppendNewLine($"FOR EACH ROW EXECUTE PROCEDURE {functionName}();");
        
        return sql;
    }

    public override string GenerateDeleteTriggerSql(string triggerName, IEntityType entityType)
    {
        var schemaName = entityType.GetSchema();
        var name = string.IsNullOrWhiteSpace(schemaName)
            ? triggerName
            : $"{schemaName}.{triggerName}";
        return SqlBuilder.FromString($"DROP FUNCTION {name}() CASCADE;");
    }
}