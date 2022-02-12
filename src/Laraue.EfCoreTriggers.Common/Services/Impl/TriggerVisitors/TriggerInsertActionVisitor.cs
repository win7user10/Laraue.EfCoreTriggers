using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors.Statements;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;

public class TriggerInsertActionVisitor : ITriggerActionVisitor<TriggerInsertAction>
{
    private readonly IInsertExpressionVisitor _visitor;
    private readonly IDbSchemaRetriever _adapter;

    public TriggerInsertActionVisitor(
        IInsertExpressionVisitor visitor,
        IDbSchemaRetriever adapter)
    {
        _visitor = visitor;
        _adapter = adapter;
    }
    
    public SqlBuilder Visit(TriggerInsertAction triggerAction, VisitedMembers visitedMembers)
    {
        var insertStatement = _visitor.Visit(
            triggerAction.InsertExpression,
            triggerAction.InsertExpressionPrefixes,
            visitedMembers);

        var insertEntityType = triggerAction.InsertExpression.Body.Type;
        
        return SqlBuilder.FromString($"INSERT INTO {_adapter.GetTableName(insertEntityType)} ")
            .Append(insertStatement)
            .Append(";");
    }
}