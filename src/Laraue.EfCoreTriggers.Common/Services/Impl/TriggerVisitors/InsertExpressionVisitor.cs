using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Services.Impl.SetExpressionVisitors;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;

public class InsertExpressionVisitor : IInsertExpressionVisitor
{
    private readonly IMemberInfoVisitorFactory _factory;
    private readonly IDbSchemaRetriever _adapter;
    private readonly ISqlGenerator _sqlGenerator;

    public InsertExpressionVisitor(
        IMemberInfoVisitorFactory factory,
        IDbSchemaRetriever adapter,
        ISqlGenerator sqlGenerator)
    {
        _factory = factory;
        _adapter = adapter;
        _sqlGenerator = sqlGenerator;
    }

    public SqlBuilder Visit(LambdaExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
    {
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
                        $"{_sqlGenerator.GetDelimiter()}{_adapter.GetColumnName(x.Key)}{_sqlGenerator.GetDelimiter()}"))
                .Append(") VALUES (")
                .AppendJoin(", ", assignmentParts
                    .Select(x => x.Value.ToString()))
                .Append(")");
        }
        else
        {
            sqlResult.Append(VisitEmptyInsertBody(expression, argumentTypes));
        }
            
        return sqlResult;
    }
    
    protected virtual SqlBuilder VisitEmptyInsertBody(LambdaExpression insertExpression, ArgumentTypes argumentTypes)
    {
        var sqlBuilder = new SqlBuilder();
        sqlBuilder.Append("DEFAULT VALUES");
        return sqlBuilder;
    }
}