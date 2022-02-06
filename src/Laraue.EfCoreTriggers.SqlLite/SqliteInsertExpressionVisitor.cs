using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Impl.SetExpressionVisitors;
using Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Laraue.EfCoreTriggers.SqlLite;

public class SqliteInsertExpressionVisitor : InsertExpressionVisitor
{
    private readonly IEfCoreMetadataRetriever _metadataRetriever;
    private readonly IReadOnlyModel _model;
    
    public SqliteInsertExpressionVisitor(
        ISetExpressionVisitorFactory factory,
        IEfCoreMetadataRetriever metadataRetriever,
        ISqlGenerator sqlGenerator,
        IReadOnlyModel model) 
        : base(factory, metadataRetriever, sqlGenerator)
    {
        _metadataRetriever = metadataRetriever;
        _model = model;
    }

    protected override SqlBuilder VisitEmptyInsertBody(LambdaExpression insertExpression, ArgumentTypes argumentTypes)
    {
        var primaryKeyProperties = GetPrimaryKeyMembers(insertExpression.Body.Type);
        var sqlBuilder = new SqlBuilder();
        sqlBuilder.Append("(")
            .AppendJoin(", ", primaryKeyProperties.Select(_metadataRetriever.GetColumnName))
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