using System.Linq;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;
using Laraue.EfCoreTriggers.Common.v2.Impl.SetExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;

public class TriggerUpsertActionVisitor : ITriggerActionVisitor<TriggerUpsertAction>
{
    private readonly ISetExpressionVisitorFactory _setExpressionVisitorFactory;
    private readonly IUpdateExpressionVisitor _updateExpressionVisitor;
    private readonly IInsertExpressionVisitor _insertExpressionVisitor;
    private readonly IEfCoreMetadataRetriever _metadataRetriever;

    public TriggerUpsertActionVisitor(
        ISetExpressionVisitorFactory setExpressionVisitorFactory,
        IUpdateExpressionVisitor updateExpressionVisitor,
        IInsertExpressionVisitor insertExpressionVisitor,
        IEfCoreMetadataRetriever metadataRetriever)
    {
        _setExpressionVisitorFactory = setExpressionVisitorFactory;
        _updateExpressionVisitor = updateExpressionVisitor;
        _insertExpressionVisitor = insertExpressionVisitor;
        _metadataRetriever = metadataRetriever;
    }

    public virtual SqlBuilder Visit(TriggerUpsertAction triggerAction, VisitedMembers visitedMembers)
    {
        var matchExpressionParts = _setExpressionVisitorFactory.Visit(
            triggerAction.MatchExpression,
            triggerAction.MatchExpressionPrefixes,
            visitedMembers);

        var updateEntityType = triggerAction.InsertExpression.Body.Type;

        var insertStatementSql = _insertExpressionVisitor.Visit(
            triggerAction.InsertExpression,
            triggerAction.InsertExpressionPrefixes,
            visitedMembers);
            
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
            var updateStatementSql = _updateExpressionVisitor.Visit(
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