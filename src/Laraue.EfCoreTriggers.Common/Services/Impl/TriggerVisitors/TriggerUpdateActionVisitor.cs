using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;
using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors.Statements;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;

public class TriggerUpdateActionVisitor : ITriggerActionVisitor<TriggerUpdateAction>
{
    private readonly IDbSchemaRetriever _dbSchemaRetriever;
    private readonly IExpressionVisitorFactory _expressionVisitorFactory;
    private readonly IUpdateExpressionVisitor _updateExpressionVisitor;

    public TriggerUpdateActionVisitor(
        IDbSchemaRetriever dbSchemaRetriever,
        IExpressionVisitorFactory expressionVisitorFactory, 
        IUpdateExpressionVisitor updateExpressionVisitor)
    {
        _dbSchemaRetriever = dbSchemaRetriever;
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
            .Append($"UPDATE {_dbSchemaRetriever.GetTableName(updateEntity)}")
            .AppendNewLine("SET ")
            .Append(updateStatement)
            .AppendNewLine("WHERE ")
            .Append(binaryExpressionSql)
            .Append(";");
    }
    
    
}