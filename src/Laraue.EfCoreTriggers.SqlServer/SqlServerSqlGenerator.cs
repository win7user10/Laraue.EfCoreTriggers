using System;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.SqlServer;

public class SqlServerSqlGenerator : SqlGenerator
{
    public SqlServerSqlGenerator(
        IDbSchemaRetriever adapter,
        SqlTypeMappings sqlTypeMappings,
        VisitingInfo visitingInfo) 
        : base(adapter, sqlTypeMappings, visitingInfo)
    {
    }

    public override string NewEntityPrefix => "Inserted";

    public override string OldEntityPrefix => "Deleted";

    protected override string GetNodeTypeSql(ExpressionType expressionType)
    {
        return expressionType switch
        {
            ExpressionType.IsTrue => $"= {GetSql(true)}",
            ExpressionType.IsFalse => $"= {GetSql(false)}",
            ExpressionType.Not => $"= {GetSql(false)}",
            _ => base.GetNodeTypeSql(expressionType)
        };
    }

    public override string GetSql(bool source)
    {
        return source ? "1" : "0";
    }

    public override string GetColumnValueReferenceSql(Type type, MemberInfo member, ArgumentType argumentType)
    {
        return argumentType switch
        {
            ArgumentType.New => $"@New{member.Name}",
            ArgumentType.Old => $"@Old{member.Name}",
            _ => throw new InvalidOperationException(
                $"Invalid attempt to generate declaring variable SQL using argument prefix {argumentType}")
        };
    }
}