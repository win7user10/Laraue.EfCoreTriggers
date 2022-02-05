using System.Linq;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;

namespace Laraue.EfCoreTriggers.MySql;

public class MySqlTriggerVisitor : BaseTriggerVisitor
{
    private readonly ITriggerActionVisitorFactory _factory;
    private readonly IEfCoreMetadataRetriever _metadataRetriever;

    public MySqlTriggerVisitor(ITriggerActionVisitorFactory factory, IEfCoreMetadataRetriever metadataRetriever)
    {
        _factory = factory;
        _metadataRetriever = metadataRetriever;
    }

    public override string GenerateCreateTriggerSql(ITrigger trigger)
    {
        var triggerTimeName = GetTriggerTimeName(trigger.TriggerTime);
        
        var actionsSql = trigger.Actions
            .Select(action => _factory.Visit(action, new VisitedMembers()))
            .ToArray();
        
        var sql = SqlBuilder.FromString($"CREATE TRIGGER {trigger.Name}")
            .AppendNewLine($"{triggerTimeName} {trigger.TriggerEvent.ToString().ToUpper()} ON {_metadataRetriever.GetTableName(trigger.TriggerEntityType)}")
            .AppendNewLine("FOR EACH ROW")
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
            .AppendNewLine("END");
        
        return sql;
    }

    public override string GenerateDeleteTriggerSql(string triggerName)
    {
        return SqlBuilder.FromString($"DROP TRIGGER {triggerName};");
    }
}