using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Laraue.EfCoreTriggers.Common.v2.Impl.SetExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;

public class TriggerUpdateActionVisitor : ITriggerActionVisitor<TriggerUpdateAction>
{
    private readonly ISetExpressionVisitorFactory _setExpressionVisitorFactory;
    private readonly IEfCoreMetadataRetriever _efCoreMetadataRetriever;

    public TriggerUpdateActionVisitor(
        IEfCoreMetadataRetriever efCoreMetadataRetriever,
        ISetExpressionVisitorFactory setExpressionVisitorFactory)
    {
        _efCoreMetadataRetriever = efCoreMetadataRetriever;
        _setExpressionVisitorFactory = setExpressionVisitorFactory;
    }

    public SqlBuilder Visit(TriggerUpdateAction triggerAction, VisitedMembers visitedMembers)
    {
        var assignmentParts = _setExpressionVisitorFactory.Visit(
            triggerAction.UpdateExpression,
            triggerAction.UpdateExpressionPrefixes,
            visitedMembers);
        
        var sqlResult = new SqlBuilder();
        
        var assignmentPartsSql = assignmentParts
            .Select(expressionPart =>
                $"{_efCoreMetadataRetriever.GetColumnName(expressionPart.Key)} = {expressionPart.Value}")
            .ToArray();
        
        sqlResult.AppendJoin(", ", assignmentPartsSql);
        return sqlResult;
    }
    
    public virtual SqlBuilder GetUpdateStatementBodySql(LambdaExpression updateExpression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
    {
        var assignmentParts = _setExpressionVisitorFactory.Visit(
            updateExpression,
            argumentTypes,
            visitedMembers);
        
        var sqlResult = new SqlBuilder();
        
        var assignmentPartsSql = assignmentParts
            .Select(expressionPart => 
                $"{_efCoreMetadataRetriever.GetColumnName(expressionPart.Key)} = {expressionPart.Value}")
            .ToArray();
        
        sqlResult.AppendJoin(", ", assignmentPartsSql);
        return sqlResult;
    }
}