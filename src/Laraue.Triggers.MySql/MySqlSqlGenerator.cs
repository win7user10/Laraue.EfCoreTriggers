using Laraue.Triggers.Core.SqlGeneration;
using Laraue.Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Triggers.MySql;

/// <inheritdoc />
public sealed class MySqlSqlGenerator : SqlGenerator
{
    /// <inheritdoc />
    public MySqlSqlGenerator(
        IDbSchemaRetriever adapter,
        SqlTypeMappings sqlTypeMappings,
        VisitingInfo visitingInfo)
        : base(adapter, sqlTypeMappings, visitingInfo)
    {
    }

    /// <inheritdoc />
    public override char GetDelimiter()
    {
        return '`';
    }
}