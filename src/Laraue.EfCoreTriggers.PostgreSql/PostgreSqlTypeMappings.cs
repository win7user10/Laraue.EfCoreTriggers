using System;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.PostgreSql;

public class PostgreSqlTypeMappings : SqlTypeMappings
{
    public PostgreSqlTypeMappings()
    {
        Add(typeof(bool), "boolean");
        Add(typeof(byte), "smallint");
        Add(typeof(short), "smallint");
        Add(typeof(int), "integer");
        Add(typeof(long), "bigint");
        Add(typeof(sbyte), "smallint");
        Add(typeof(uint), "oid");
        Add(typeof(decimal), "money");
        Add(typeof(float), "real");
        Add(typeof(double), "double precision");
        Add(typeof(Enum), "integer");
        Add(typeof(char), "(internal) char");
        Add(typeof(string), "name");
        Add(typeof(DateTime), "timestamp without time zone");
        Add(typeof(DateTimeOffset), "time with time zone");
        Add(typeof(TimeSpan), "time without time zone");
        Add(typeof(Guid), "uuid");
    }
}