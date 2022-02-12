using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Services.Impl.SetExpressionVisitors;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors.Statements;

/// <inheritdoc />
public class UpdateExpressionVisitor : IUpdateExpressionVisitor
{
    private readonly IMemberInfoVisitorFactory _memberInfoVisitorFactory;
    private readonly IDbSchemaRetriever _dbSchemaRetriever;

    /// <summary>
    /// Initializes a new instance of <see cref="UpdateExpressionVisitor"/>.
    /// </summary>
    /// <param name="memberInfoVisitorFactory"></param>
    /// <param name="dbSchemaRetriever"></param>
    public UpdateExpressionVisitor(
        IMemberInfoVisitorFactory memberInfoVisitorFactory,
        IDbSchemaRetriever dbSchemaRetriever)
    {
        _memberInfoVisitorFactory = memberInfoVisitorFactory;
        _dbSchemaRetriever = dbSchemaRetriever;
    }

    /// <inheritdoc />
    public SqlBuilder Visit(LambdaExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
    {
        var assignmentParts = _memberInfoVisitorFactory.Visit(
            expression,
            argumentTypes,
            visitedMembers);
        
        var sqlResult = new SqlBuilder();
        
        var assignmentPartsSql = assignmentParts
            .Select(expressionPart => 
                $"{_dbSchemaRetriever.GetColumnName(expressionPart.Key)} = {expressionPart.Value}")
            .ToArray();
        
        sqlResult.AppendJoin(", ", assignmentPartsSql);
        return sqlResult;
    }
}