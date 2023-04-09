using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

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