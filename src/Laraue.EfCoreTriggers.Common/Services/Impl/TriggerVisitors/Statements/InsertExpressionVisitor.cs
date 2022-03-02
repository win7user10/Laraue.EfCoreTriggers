using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Services.Impl.SetExpressionVisitors;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors.Statements;

/// <inheritdoc />
public class InsertExpressionVisitor : IInsertExpressionVisitor
{
    private readonly IMemberInfoVisitorFactory _factory;
    private readonly IDbSchemaRetriever _adapter;
    private readonly ISqlGenerator _sqlGenerator;

    /// <summary>
    /// Initializes a new instance of <see cref="InsertExpressionVisitor"/>.
    /// </summary>
    /// <param name="factory"></param>
    /// <param name="adapter"></param>
    /// <param name="sqlGenerator"></param>
    public InsertExpressionVisitor(
        IMemberInfoVisitorFactory factory,
        IDbSchemaRetriever adapter,
        ISqlGenerator sqlGenerator)
    {
        _factory = factory;
        _adapter = adapter;
        _sqlGenerator = sqlGenerator;
    }

    /// <inheritdoc />
    public SqlBuilder Visit(LambdaExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
    {
        var insertType = expression.Body.Type;
        
        var assignmentParts = _factory.Visit(
            expression,
            argumentTypes,
            visitedMembers);
        
        var sqlResult = new SqlBuilder();

        if (assignmentParts.Any())
        {
            sqlResult.Append("(")
                .AppendJoin(", ", assignmentParts
                    .Select(x =>
                        $"{_sqlGenerator.GetDelimiter()}{_adapter.GetColumnName(insertType, x.Key)}{_sqlGenerator.GetDelimiter()}"))
                .Append(") SELECT ");

            sqlResult.AppendViaNewLine(", ", assignmentParts
                .Select(x => x.Value));
        }
        else
        {
            sqlResult.Append(VisitEmptyInsertBody(expression, argumentTypes));
        }
            
        return sqlResult;
    }
    
    /// <summary>
    /// Get SQL for the empty insert statement.
    /// </summary>
    /// <param name="insertExpression"></param>
    /// <param name="argumentTypes"></param>
    /// <returns></returns>
    protected virtual SqlBuilder VisitEmptyInsertBody(LambdaExpression insertExpression, ArgumentTypes argumentTypes)
    {
        var sqlBuilder = new SqlBuilder();
        sqlBuilder.Append("DEFAULT VALUES");
        return sqlBuilder;
    }
}