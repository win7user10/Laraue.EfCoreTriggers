using System;
using Laraue.Linq2Triggers.Core.SqlGeneration;

namespace Laraue.Linq2Triggers.Providers.Oracle;

/// <inheritdoc />
public class OracleTypeMappings : SqlTypeMappings
{
    /// <inheritdoc />
    public OracleTypeMappings()
    {
        Add(typeof(bool), "BIT(1)");
        Add(typeof(byte), "TINYINT UNSIGNED");
        Add(typeof(short), "SMALLINT");
        Add(typeof(int), "INT");
        Add(typeof(long), "BIGINT");
        Add(typeof(sbyte), "TINYINT");
        Add(typeof(decimal), "DECIMAL");
        Add(typeof(float), "FLOAT");
        Add(typeof(double), "DOUBLE");
        Add(typeof(Enum), "INT");
        Add(typeof(char), "CHAR");
        Add(typeof(string), "TEXT");
        Add(typeof(DateTime), "DATETIME");
        Add(typeof(TimeSpan), "TIME");
        Add(typeof(Guid), "CHAR(36)");
    }
}