using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.Services.Impl;

namespace Laraue.EfCoreTriggers.SqlServer;

public class SqlServerSqlGenerator : SqlGenerator
{
    public SqlServerSqlGenerator(IDbSchemaRetriever adapter, SqlTypeMappings sqlTypeMappings) 
        : base(adapter, sqlTypeMappings)
    {
        
    }

    public override string NewEntityPrefix => "Inserted";

    public override string OldEntityPrefix => "Deleted";

    public override string GetOperand(Expression expression)
    {
        return expression.NodeType switch
        {
            ExpressionType.IsTrue => $"= {GetSql(true)}",
            ExpressionType.IsFalse => $"= {GetSql(false)}",
            ExpressionType.Not => $"= {GetSql(false)}",
            _ => base.GetOperand(expression)
        };
    }

    public override string GetSql(bool source)
    {
        return source ? "1" : "0";
    }
}