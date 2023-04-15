using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.Visitors.SetExpressionVisitors;
using Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors.Statements;

namespace Laraue.EfCoreTriggers.SqlLite;

/// <inheritdoc />
public sealed class SqliteInsertExpressionVisitor : InsertExpressionVisitor
{
    private readonly IDbSchemaRetriever _adapter;
    private readonly ISqlGenerator _sqlGenerator;

    /// <inheritdoc />
    public SqliteInsertExpressionVisitor(
        IMemberInfoVisitorFactory factory,
        IDbSchemaRetriever adapter,
        ISqlGenerator sqlGenerator) 
        : base(factory, sqlGenerator)
    {
        _adapter = adapter;
        _sqlGenerator = sqlGenerator;
    }

    /// <inheritdoc />
    protected override SqlBuilder VisitEmptyInsertBody(LambdaExpression insertExpression)
    {
        var insertType = insertExpression.Body.Type;
        
        var primaryKeyProperties = _adapter.GetPrimaryKeyMembers(insertType);
        
        var sqlBuilder = SqlBuilder.FromString("(")
            .AppendJoin(", ", primaryKeyProperties
                .Select(propertyInfo => _sqlGenerator
                    .GetColumnSql(insertType, propertyInfo, ArgumentType.None)))
            .Append(") VALUES (")
            .AppendJoin(", ", primaryKeyProperties.Select(_ => "NULL"))
            .Append(")");

        return sqlBuilder;
    }
}