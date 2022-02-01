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

        var triggerEntityType = trigger.Actions
            .Select(x => x.GetEntityType())
            .First(x => x is not null);
        
        var sql = SqlBuilder.FromString($"CREATE TRIGGER {trigger.Name}")
            .AppendNewLine($"{triggerTimeName} {trigger.TriggerEvent.ToString().ToUpper()} ON {_metadataRetriever.GetTableName(trigger.TriggerEntityType)}")
            .AppendNewLine("FOR EACH ROW")
            .AppendNewLine("BEGIN")
            .WithIdent(loopSql =>
            {
                loopSql.AppendViaNewLine(actionsSql);
            })
            .AppendNewLine("END");
        
        return sql;
    }

    public override string GenerateDeleteTriggerSql(string triggerName)
    {
        return new SqlBuilder().Append($"DROP TRIGGER {triggerName};");
    }
}