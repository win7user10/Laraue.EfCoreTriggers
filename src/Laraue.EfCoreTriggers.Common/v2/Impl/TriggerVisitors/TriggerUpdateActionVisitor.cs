using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;
using Laraue.EfCoreTriggers.Common.v2.Impl.SetExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;

public class TriggerUpdateActionVisitor : ITriggerActionVisitor<TriggerUpdateAction>
{
    private readonly IEfCoreMetadataRetriever _efCoreMetadataRetriever;
    private readonly IExpressionVisitorFactory _expressionVisitorFactory;
    private readonly IUpdateExpressionVisitor _updateExpressionVisitor;

    public TriggerUpdateActionVisitor(
        IEfCoreMetadataRetriever efCoreMetadataRetriever,
        IExpressionVisitorFactory expressionVisitorFactory, 
        IUpdateExpressionVisitor updateExpressionVisitor)
    {
        _efCoreMetadataRetriever = efCoreMetadataRetriever;
        _expressionVisitorFactory = expressionVisitorFactory;
        _updateExpressionVisitor = updateExpressionVisitor;
    }

    public SqlBuilder Visit(TriggerUpdateAction triggerAction, VisitedMembers visitedMembers)
    {
        var updateStatement = _updateExpressionVisitor.Visit(
            triggerAction.UpdateExpression,
            triggerAction.UpdateExpressionPrefixes,
            visitedMembers);
        
        var binaryExpressionSql = _expressionVisitorFactory.Visit(
            (BinaryExpression)triggerAction.UpdateFilter.Body,
            triggerAction.UpdateFilterPrefixes,
            visitedMembers);

        var updateEntity = triggerAction.UpdateExpression.Body.Type;

        return new SqlBuilder()
            .Append($"UPDATE {_efCoreMetadataRetriever.GetTableName(updateEntity)}")
            .AppendNewLine("SET ")
            .Append(updateStatement)
            .AppendNewLine("WHERE ")
            .Append(binaryExpressionSql)
            .Append(";");
    }
    
    
}