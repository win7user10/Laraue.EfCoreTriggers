using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Impl;

namespace Laraue.EfCoreTriggers.SqlServer;

public class SqlServerSqlGenerator : SqlGenerator
{
    public SqlServerSqlGenerator(IEfCoreMetadataRetriever metadataRetriever, SqlTypeMappings sqlTypeMappings) 
        : base(metadataRetriever, sqlTypeMappings)
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