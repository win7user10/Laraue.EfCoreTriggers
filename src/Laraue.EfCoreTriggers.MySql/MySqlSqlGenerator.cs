using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.Services.Impl;
using Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.MySql;

public class MySqlSqlGenerator : SqlGenerator
{
    public MySqlSqlGenerator(
        IDbSchemaRetriever adapter,
        SqlTypeMappings sqlTypeMappings,
        VisitingInfo visitingInfo)
        : base(adapter, sqlTypeMappings, visitingInfo)
    {
    }

    public override char GetDelimiter()
    {
        return '`';
    }
}