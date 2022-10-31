using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.Services.Impl.SetExpressionVisitors;
using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors.Statements;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.SqlLite;

public class SqliteInsertExpressionVisitor : InsertExpressionVisitor
{
    private readonly IDbSchemaRetriever _adapter;
    private readonly ISqlGenerator _sqlGenerator;

    public SqliteInsertExpressionVisitor(
        IMemberInfoVisitorFactory factory,
        IDbSchemaRetriever adapter,
        ISqlGenerator sqlGenerator) 
        : base(factory, sqlGenerator)
    {
        _adapter = adapter;
        _sqlGenerator = sqlGenerator;
    }

    protected override SqlBuilder VisitEmptyInsertBody(LambdaExpression insertExpression, ArgumentTypes argumentTypes)
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