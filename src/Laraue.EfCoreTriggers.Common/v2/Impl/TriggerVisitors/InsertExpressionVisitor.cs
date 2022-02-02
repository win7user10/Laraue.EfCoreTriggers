using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.v2.Impl.SetExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;

public class InsertExpressionVisitor : IInsertExpressionVisitor
{
    private readonly ISetExpressionVisitorFactory _factory;
    private readonly IEfCoreMetadataRetriever _metadataRetriever;
    private readonly ISqlGenerator _sqlGenerator;

    public InsertExpressionVisitor(
        ISetExpressionVisitorFactory factory,
        IEfCoreMetadataRetriever metadataRetriever,
        ISqlGenerator sqlGenerator)
    {
        _factory = factory;
        _metadataRetriever = metadataRetriever;
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
                        $"{_sqlGenerator.GetDelimiter()}{_metadataRetriever.GetColumnName(x.Key)}{_sqlGenerator.GetDelimiter()}"))
                .Append(")")
                .AppendNewLine("VALUES (")
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