using System.Linq;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;

public class TriggerDeleteActionVisitor : ITriggerActionVisitor<TriggerDeleteAction>
{
    private readonly IEfCoreMetadataRetriever _efCoreMetadataRetriever;
    private readonly ITriggerActionVisitorFactory _factory;

    public TriggerDeleteActionVisitor(IEfCoreMetadataRetriever efCoreMetadataRetriever, ITriggerActionVisitorFactory factory)
    {
        _efCoreMetadataRetriever = efCoreMetadataRetriever;
        _factory = factory;
    }

    public SqlBuilder Visit(TriggerDeleteAction triggerAction, VisitedMembers visitedMembers)
    {
        var tableType = triggerAction.DeletePredicate.Parameters.Last().Type;

        var triggerCondition = new TriggerCondition(triggerAction.DeletePredicate, triggerAction.DeleteFilterPrefixes);
        var conditionStatement = _factory.Visit(triggerCondition, visitedMembers);
        
        return new SqlBuilder()
            .Append($"DELETE FROM {_efCoreMetadataRetriever.GetTableName(tableType)}")
            .AppendNewLine("WHERE ")
            .Append(conditionStatement)
            .Append(";");
    }
}