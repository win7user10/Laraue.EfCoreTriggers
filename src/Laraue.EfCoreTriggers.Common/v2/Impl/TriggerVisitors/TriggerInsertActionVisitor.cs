using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Laraue.EfCoreTriggers.Common.v2.Impl.SetExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;

public class TriggerInsertActionVisitor : ITriggerActionVisitor<TriggerInsertAction>
{
    private readonly ISetExpressionVisitorFactory _factory;
    private readonly IEfCoreMetadataRetriever _metadataRetriever;
    private readonly ISqlGenerator _sqlGenerator;

    public TriggerInsertActionVisitor(
        ISetExpressionVisitorFactory factory,
        IEfCoreMetadataRetriever metadataRetriever,
        ISqlGenerator sqlGenerator)
    {
        _factory = factory;
        _metadataRetriever = metadataRetriever;
        _sqlGenerator = sqlGenerator;
    }
    
    public SqlBuilder Visit(TriggerInsertAction triggerAction, VisitedMembers visitedMembers)
    {
        var insertStatement = GetInsertStatementSql(
            triggerAction.InsertExpression,
            triggerAction.InsertExpressionPrefixes,
            visitedMembers);

        var insertEntityType = triggerAction.InsertExpression.Body.Type;
        
        return SqlBuilder.FromString($"INSERT INTO {_metadataRetriever.GetTableName(insertEntityType)} ")
            .Append(insertStatement)
            .Append(";");
    }

    public SqlBuilder GetInsertStatementSql(LambdaExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
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
    
    public virtual SqlBuilder VisitEmptyInsertBody(LambdaExpression insertExpression, ArgumentTypes argumentTypes)
    {
        var sqlBuilder = new SqlBuilder();
        sqlBuilder.Append("DEFAULT VALUES");
        return sqlBuilder;
    }
}