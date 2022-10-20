using System.Linq;
using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Laraue.EfCoreTriggers.MySql;

public class MySqlTriggerVisitor : BaseTriggerVisitor
{
    private readonly ITriggerActionVisitorFactory _factory;
    private readonly IDbSchemaRetriever _adapter;

    public MySqlTriggerVisitor(ITriggerActionVisitorFactory factory, IDbSchemaRetriever adapter)
    {
        _factory = factory;
        _adapter = adapter;
    }

    public override string GenerateCreateTriggerSql(ITrigger trigger)
    {
        var triggerTimeName = GetTriggerTimeName(trigger.TriggerTime);
        
        var actionsSql = trigger.Actions
            .Select(action => _factory.Visit(action, new VisitedMembers()))
            .ToArray();
        
        var sql = SqlBuilder.FromString($"CREATE TRIGGER {trigger.Name}")
            .AppendNewLine($"{triggerTimeName} {trigger.TriggerEvent.ToString().ToUpper()} ON {_adapter.GetTableName(trigger.TriggerEntityType)}")
            .AppendNewLine("FOR EACH ROW")
            .AppendNewLine("BEGIN")
            .WithIdent(triggerSql =>
            {
                var isAnyCondition = trigger.Conditions.Count > 0;
                
                if (isAnyCondition)
                {
                    var conditionsSql = trigger.Conditions
                        .Select(actionCondition => _factory.Visit(actionCondition, new VisitedMembers()));
            
                    triggerSql.AppendNewLine($"IF ")
                        .AppendJoin(" AND ", conditionsSql.Select(x => x.ToString()))
                        .Append(" THEN ");
                }

                triggerSql.WithIdentWhen(isAnyCondition,  loopSql => loopSql.AppendViaNewLine(actionsSql));
        
                if (trigger.Conditions.Count > 0)
                {
                    triggerSql.AppendNewLine($"END IF;");
                }
            })
            .AppendNewLine("END");
        
        return sql;
    }

    public override string GenerateDeleteTriggerSql(string triggerName, IEntityType entityType)
    {
        return SqlBuilder.FromString($"DROP TRIGGER {triggerName};");
    }
}