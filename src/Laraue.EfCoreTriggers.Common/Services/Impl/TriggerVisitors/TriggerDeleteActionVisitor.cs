using System.Linq;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;

public class TriggerDeleteActionVisitor : ITriggerActionVisitor<TriggerDeleteAction>
{
    private readonly IDbSchemaRetriever _dbSchemaRetriever;
    private readonly ITriggerActionVisitorFactory _factory;

    public TriggerDeleteActionVisitor(IDbSchemaRetriever dbSchemaRetriever, ITriggerActionVisitorFactory factory)
    {
        _dbSchemaRetriever = dbSchemaRetriever;
        _factory = factory;
    }

    public SqlBuilder Visit(TriggerDeleteAction triggerAction, VisitedMembers visitedMembers)
    {
        var tableType = triggerAction.DeletePredicate.Parameters.Last().Type;

        var triggerCondition = new TriggerCondition(triggerAction.DeletePredicate, triggerAction.DeleteFilterPrefixes);
        var conditionStatement = _factory.Visit(triggerCondition, visitedMembers);
        
        return new SqlBuilder()
            .Append($"DELETE FROM {_dbSchemaRetriever.GetTableName(tableType)}")
            .AppendNewLine("WHERE ")
            .Append(conditionStatement)
            .Append(";");
    }
}