using System.Linq;
using Laraue.EfCoreTriggers.Common.Services.Impl.SetExpressionVisitors;
using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors.Statements;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;

public class TriggerUpsertActionVisitor : ITriggerActionVisitor<TriggerUpsertAction>
{
    private readonly IMemberInfoVisitorFactory _memberInfoVisitorFactory;
    private readonly IUpdateExpressionVisitor _updateExpressionVisitor;
    private readonly IInsertExpressionVisitor _insertExpressionVisitor;
    private readonly IDbSchemaRetriever _adapter;

    public TriggerUpsertActionVisitor(
        IMemberInfoVisitorFactory memberInfoVisitorFactory,
        IUpdateExpressionVisitor updateExpressionVisitor,
        IInsertExpressionVisitor insertExpressionVisitor,
        IDbSchemaRetriever adapter)
    {
        _memberInfoVisitorFactory = memberInfoVisitorFactory;
        _updateExpressionVisitor = updateExpressionVisitor;
        _insertExpressionVisitor = insertExpressionVisitor;
        _adapter = adapter;
    }

    public virtual SqlBuilder Visit(TriggerUpsertAction triggerAction, VisitedMembers visitedMembers)
    {
        var matchExpressionParts = _memberInfoVisitorFactory.Visit(
            triggerAction.MatchExpression,
            triggerAction.MatchExpressionPrefixes,
            visitedMembers);

        var updateEntityType = triggerAction.InsertExpression.Body.Type;

        var insertStatementSql = _insertExpressionVisitor.Visit(
            triggerAction.InsertExpression,
            triggerAction.InsertExpressionPrefixes,
            visitedMembers);
            
        var sqlBuilder = SqlBuilder.FromString($"INSERT INTO {_adapter.GetTableName(updateEntityType)} ")
            .Append(insertStatementSql)
            .Append(" ON CONFLICT (")
            .AppendJoin(", ", matchExpressionParts
                .Select(x => 
                    _adapter.GetColumnName(updateEntityType, x.Key)))
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