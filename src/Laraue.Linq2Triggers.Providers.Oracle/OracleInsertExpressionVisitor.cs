using System.Linq;
using System.Linq.Expressions;
using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.TriggerBuilders;
using Laraue.Linq2Triggers.Core.Visitors.SetExpressionVisitors;
using Laraue.Linq2Triggers.Core.Visitors.TriggerVisitors.Statements;

namespace Laraue.Linq2Triggers.Providers.Oracle;

/// <inheritdoc />
public sealed class OracleInsertExpressionVisitor : InsertExpressionVisitor
{
    private readonly ISqlGenerator _sqlGenerator;
    private readonly IDbSchemaRetriever _dbSchemaRetriever;
    
    /// <inheritdoc />
    public OracleInsertExpressionVisitor(
        IMemberInfoVisitorFactory factory,
        ISqlGenerator sqlGenerator,
        IDbSchemaRetriever dbSchemaRetriever) 
        : base(factory, sqlGenerator)
    {
        _sqlGenerator = sqlGenerator;
        _dbSchemaRetriever = dbSchemaRetriever;
    }

    /// <inheritdoc />
    protected override SqlBuilder VisitEmptyInsertBody(LambdaExpression insertExpression)
    {
        var insertType = insertExpression.Body.Type;
        
        var primaryKeyProperties = _dbSchemaRetriever.GetPrimaryKeyMembers(insertType);
        
        var sqlBuilder = SqlBuilder.FromString("(")
            .AppendJoin(", ", primaryKeyProperties
                .Select(propertyInfo => _sqlGenerator
                    .GetColumnSql(insertType, propertyInfo.Name, ArgumentType.None)))
            .Append(") VALUES (")
            .AppendJoin(", ", primaryKeyProperties.Select(_ => "DEFAULT"))
            .Append(")");

        return sqlBuilder;
    }
}