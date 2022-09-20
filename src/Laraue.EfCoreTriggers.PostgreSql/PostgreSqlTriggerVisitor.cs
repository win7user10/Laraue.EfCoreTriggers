using System.Linq;
using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.PostgreSql;

public class PostgreSqlTriggerVisitor : BaseTriggerVisitor
{
    private readonly ITriggerActionVisitorFactory _factory;
    private readonly IDbSchemaRetriever _adapter;

    public PostgreSqlTriggerVisitor(ITriggerActionVisitorFactory factory, IDbSchemaRetriever adapter)
    {
        _factory = factory;
        _adapter = adapter;
    }

    public override string GenerateCreateTriggerSql(ITrigger trigger)
    {
        var actionsSql = trigger.Actions
            .Select(action => _factory.Visit(action, new VisitedMembers()))
            .ToArray();

        var sql = SqlBuilder.FromString($"CREATE FUNCTION {_adapter.GetFunctionName(trigger.TriggerEntityType, trigger.Name)}() RETURNS trigger as ${trigger.Name}$")
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
            .AppendNewLine($"ON {_adapter.GetTableName(trigger.TriggerEntityType)}")
            .AppendNewLine($"FOR EACH ROW EXECUTE PROCEDURE {_adapter.GetFunctionName(trigger.TriggerEntityType, trigger.Name)}();");
        
        return sql;
    }

    public override string GenerateDeleteTriggerSql(string triggerName)
    {
        return SqlBuilder.FromString($"DROP FUNCTION {triggerName}() CASCADE;");
    }
}