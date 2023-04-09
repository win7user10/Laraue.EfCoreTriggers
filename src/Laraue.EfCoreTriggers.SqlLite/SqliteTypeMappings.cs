using System;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.SqlLite;

public class SqliteTypeMappings : SqlTypeMappings
{
    public SqliteTypeMappings()
    {
        Add(typeof(bool), "NUMERIC");
        Add(typeof(byte), "NUMERIC");
        Add(typeof(short), "NUMERIC");
        Add(typeof(int), "NUMERIC");
        Add(typeof(long), "NUMERIC");
        Add(typeof(sbyte), "NUMERIC");
        Add(typeof(uint), "NUMERIC");
        Add(typeof(decimal), "NUMERIC");
        Add(typeof(float), "REAL");
        Add(typeof(double), "REAL");
        Add(typeof(Enum), "NUMERIC");
        Add(typeof(char), "TEXT");
        Add(typeof(string), "TEXT");
        Add(typeof(DateTime), "TEXT");
        Add(typeof(DateTimeOffset), "TEXT");
        Add(typeof(TimeSpan), "TEXT");
        Add(typeof(Guid), "TEXT");
    }
}