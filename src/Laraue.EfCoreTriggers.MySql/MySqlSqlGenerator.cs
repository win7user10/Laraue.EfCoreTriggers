using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.MySql;

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