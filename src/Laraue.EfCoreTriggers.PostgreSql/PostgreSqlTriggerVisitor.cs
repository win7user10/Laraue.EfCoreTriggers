using System.Linq;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;

namespace Laraue.EfCoreTriggers.PostgreSql;

public class PostgreSqlTriggerVisitor : BaseTriggerVisitor
{
    private readonly ITriggerActionVisitorFactory _factory;
    private readonly IEfCoreMetadataRetriever _metadataRetriever;

    public PostgreSqlTriggerVisitor(ITriggerActionVisitorFactory factory, IEfCoreMetadataRetriever metadataRetriever)
    {
        _factory = factory;
        _metadataRetriever = metadataRetriever;
    }

    public override string GenerateCreateTriggerSql(ITrigger trigger)
    {
        var actionsSql = trigger.Actions
            .Select(action => _factory.Visit(action, new VisitedMembers()))
            .ToArray();

        var sql = SqlBuilder.FromString($"CREATE FUNCTION {trigger.Name}() RETURNS trigger as ${trigger.Name}$")
            .AppendNewLine("BEGIN")
            .WithIdent(triggerSql =>
            {
                if (trigger.Conditions.Count > 0)
                {
                    var conditionsSql = trigger.Conditions
                        .Select(actionCondition => _factory.Visit(actionCondition, new VisitedMembers()));
            
                    triggerSql.AppendNewLine($"IF ")
                        .AppendJoin(" AND ", conditionsSql.Select(x => x.ToString()))
                        .Append(" THEN ");
                }

                triggerSql.WithIdent(loopSql => loopSql.AppendViaNewLine(actionsSql));
        
                if (trigger.Conditions.Count > 0)
                {
                    triggerSql.AppendNewLine($"END IF;");
                }
            })
            .AppendNewLine("RETURN NEW;")
            .AppendNewLine("END;")
            .AppendNewLine($"${trigger.Name}$ LANGUAGE plpgsql;")
            .AppendNewLine($"CREATE TRIGGER {trigger.Name} {GetTriggerTimeName(trigger.TriggerTime)} {trigger.TriggerEvent.ToString().ToUpper()}")
            .AppendNewLine($"ON \"{_metadataRetriever.GetTableName(trigger.TriggerEntityType)}\"")
            .AppendNewLine($"FOR EACH ROW EXECUTE PROCEDURE {trigger.Name}();");
        
        return sql;
    }

    public override string GenerateDeleteTriggerSql(string triggerName)
    {
        return SqlBuilder.FromString($"DROP FUNCTION {triggerName}() CASCADE;");
    }
}