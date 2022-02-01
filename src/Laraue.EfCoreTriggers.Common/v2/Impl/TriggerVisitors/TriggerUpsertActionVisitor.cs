using System.Linq;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Laraue.EfCoreTriggers.Common.v2.Impl.SetExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;

public class TriggerUpsertActionVisitor : ITriggerActionVisitor<TriggerUpsertAction>
{
    private readonly ISetExpressionVisitorFactory _factory;
    private readonly TriggerInsertActionVisitor _insertActionVisitor;
    private readonly TriggerUpdateActionVisitor _updateActionVisitor;
    private readonly IEfCoreMetadataRetriever _metadataRetriever;

    public TriggerUpsertActionVisitor(
        ISetExpressionVisitorFactory factory,
        TriggerInsertActionVisitor insertActionVisitor,
        TriggerUpdateActionVisitor updateActionVisitor,
        IEfCoreMetadataRetriever metadataRetriever)
    {
        _factory = factory;
        _insertActionVisitor = insertActionVisitor;
        _updateActionVisitor = updateActionVisitor;
        _metadataRetriever = metadataRetriever;
    }

    public virtual SqlBuilder Visit(TriggerUpsertAction triggerAction, VisitedMembers visitedMembers)
    {
        var matchExpressionParts = _factory.Visit(
            triggerAction.MatchExpression,
            triggerAction.MatchExpressionPrefixes,
            visitedMembers);
        
        var insertStatementSql = _insertActionVisitor.GetInsertStatementSql(
            triggerAction.InsertExpression,
            triggerAction.InsertExpressionPrefixes,
            visitedMembers);

        var updateEntityType = triggerAction.InsertExpression.Parameters[0].Type;
            
        var sqlBuilder = SqlBuilder.FromString($"INSERT INTO {_metadataRetriever.GetTableName(updateEntityType)} ")
            .Append(insertStatementSql)
            .Append(" ON CONFLICT (")
            .AppendJoin(", ", matchExpressionParts
                .Select(x => 
                    _metadataRetriever.GetColumnName(x.Key)))
            .Append(")");

        if (triggerAction.OnMatchExpression is null)
        {
            sqlBuilder.Append(" DO NOTHING;");
        }
        else
        {
            var updateStatementSql = _updateActionVisitor.GetUpdateStatementBodySql(
                triggerAction.OnMatchExpression, 
                triggerAction.OnMatchExpressionPrefixes,
                visitedMembers);
            
            sqlBuilder.Append(" DO UPDATE SET ")
                .Append(updateStatementSql)
                .Append(";");
        }

        return sqlBuilder;
    }
}