using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.Services.Impl.SetExpressionVisitors;
using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Laraue.EfCoreTriggers.SqlLite;

public class SqliteInsertExpressionVisitor : InsertExpressionVisitor
{
    private readonly IDbSchemaRetriever _adapter;
    private readonly IReadOnlyModel _model;
    
    public SqliteInsertExpressionVisitor(
        IMemberInfoVisitorFactory factory,
        IDbSchemaRetriever adapter,
        ISqlGenerator sqlGenerator,
        IReadOnlyModel model) 
        : base(factory, adapter, sqlGenerator)
    {
        _adapter = adapter;
        _model = model;
    }

    protected override SqlBuilder VisitEmptyInsertBody(LambdaExpression insertExpression, ArgumentTypes argumentTypes)
    {
        var primaryKeyProperties = GetPrimaryKeyMembers(insertExpression.Body.Type);
        var sqlBuilder = new SqlBuilder();
        sqlBuilder.Append("(")
            .AppendJoin(", ", primaryKeyProperties.Select(_adapter.GetColumnName))
            .Append(") VALUES (")
            .AppendJoin(", ", primaryKeyProperties.Select(_ => "NULL"))
            .Append(")");

        return sqlBuilder;
    }
    
    private PropertyInfo[] GetPrimaryKeyMembers(Type type)
    {
        var entityType = _model.FindEntityType(type);
        
        return entityType.FindPrimaryKey()
            .Properties
            .Select(x => x.PropertyInfo)
            .ToArray();
    }
}