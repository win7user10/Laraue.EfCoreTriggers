using System;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.TriggerBuilders;
using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Linq2Triggers.Providers.SqlServer;

public class SqlServerSqlGenerator : SqlGenerator
{
    public SqlServerSqlGenerator(
        IDbSchemaRetriever adapter,
        SqlTypeMappings sqlTypeMappings,
        VisitingInfo visitingInfo) 
        : base(adapter, sqlTypeMappings, visitingInfo)
    {
    }

    /// <inheritdoc />
    public override string NewEntityPrefix => "Inserted";

    /// <inheritdoc />
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

    /// <inheritdoc />
    public override string GetSql(bool source)
    {
        return source ? "1" : "0";
    }

    /// <inheritdoc />
    public override string GetColumnValueReferenceSql(Type type, string memberName, ArgumentType argumentType)
    {
        return argumentType switch
        {
            ArgumentType.New => $"@New{memberName}",
            ArgumentType.Old => $"@Old{memberName}",
            _ => throw new InvalidOperationException(
                $"Invalid attempt to generate declaring variable SQL using argument prefix {argumentType}")
        };
    }
}