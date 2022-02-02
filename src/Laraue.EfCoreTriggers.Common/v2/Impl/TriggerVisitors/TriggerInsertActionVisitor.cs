using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Laraue.EfCoreTriggers.Common.v2.Impl.SetExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;

public class TriggerInsertActionVisitor : ITriggerActionVisitor<TriggerInsertAction>
{
    private readonly IInsertExpressionVisitor _visitor;
    private readonly IEfCoreMetadataRetriever _metadataRetriever;

    public TriggerInsertActionVisitor(
        IInsertExpressionVisitor visitor,
        IEfCoreMetadataRetriever metadataRetriever)
    {
        _visitor = visitor;
        _metadataRetriever = metadataRetriever;
    }
    
    public SqlBuilder Visit(TriggerInsertAction triggerAction, VisitedMembers visitedMembers)
    {
        var insertStatement = _visitor.Visit(
            triggerAction.InsertExpression,
            triggerAction.InsertExpressionPrefixes,
            visitedMembers);

        var insertEntityType = triggerAction.InsertExpression.Body.Type;
        
        return SqlBuilder.FromString($"INSERT INTO {_metadataRetriever.GetTableName(insertEntityType)} ")
            .Append(insertStatement)
            .Append(";");
    }
}