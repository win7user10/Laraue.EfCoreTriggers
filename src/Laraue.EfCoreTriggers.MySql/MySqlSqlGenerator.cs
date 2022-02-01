using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Impl;

namespace Laraue.EfCoreTriggers.MySql;

public class MySqlSqlGenerator : SqlGenerator
{
    public MySqlSqlGenerator(IEfCoreMetadataRetriever metadataRetriever, SqlTypeMappings sqlTypeMappings)
        : base(metadataRetriever, sqlTypeMappings)
    {
    }

    public override char GetDelimiter()
    {
        return '`';
    }
}