using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.Services.Impl;

namespace Laraue.EfCoreTriggers.MySql;

public class MySqlSqlGenerator : SqlGenerator
{
    public MySqlSqlGenerator(IDbSchemaRetriever adapter, SqlTypeMappings sqlTypeMappings)
        : base(adapter, sqlTypeMappings)
    {
    }

    public override char GetDelimiter()
    {
        return '`';
    }
}