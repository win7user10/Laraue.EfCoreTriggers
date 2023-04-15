using System;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.SqlServer;

public class SqlServerTypeMappings : SqlTypeMappings
{
    public SqlServerTypeMappings()
    {
        Add(typeof(bool), "BIT");
        Add(typeof(byte), "TINYINT");
        Add(typeof(short), "SMALLINT");
        Add(typeof(int), "INT");
        Add(typeof(long), "BIGINT");
        Add(typeof(sbyte), "SMALLMONEY");
        Add(typeof(ushort), "NUMERIC(20)");
        Add(typeof(uint), "NUMERIC(28)");
        Add(typeof(ulong), "NUMERIC(29)");
        Add(typeof(decimal), "DECIMAL(18,2)");
        Add(typeof(float), "FLOAT(24)");
        Add(typeof(double), "FLOAT");
        Add(typeof(Enum), "INT");
        Add(typeof(char), "CHAR(1)");
        Add(typeof(string), "NVARCHAR(MAX)");
        Add(typeof(DateTime), "DATETIME2");
        Add(typeof(DateTimeOffset), "DATETIMEOFFSET");
        Add(typeof(TimeSpan), "TIME");
        Add(typeof(Guid), "UNIQUEIDENTIFIER");
    }
}