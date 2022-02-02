using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.v2.Impl.SetExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;

public class UpdateExpressionVisitor : IUpdateExpressionVisitor
{
    private readonly ISetExpressionVisitorFactory _setExpressionVisitorFactory;
    private readonly IEfCoreMetadataRetriever _efCoreMetadataRetriever;

    public UpdateExpressionVisitor(
        ISetExpressionVisitorFactory setExpressionVisitorFactory,
        IEfCoreMetadataRetriever efCoreMetadataRetriever)
    {
        _setExpressionVisitorFactory = setExpressionVisitorFactory;
        _efCoreMetadataRetriever = efCoreMetadataRetriever;
    }

    public SqlBuilder Visit(LambdaExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
    {
        var assignmentParts = _setExpressionVisitorFactory.Visit(
            expression,
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