using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Linq2Triggers.Providers.Oracle;

/// <inheritdoc />
public sealed class OracleSqlGenerator : SqlGenerator
{
    /// <inheritdoc />
    public OracleSqlGenerator(
        IDbSchemaRetriever adapter,
        SqlTypeMappings sqlTypeMappings,
        VisitingInfo visitingInfo)
        : base(adapter, sqlTypeMappings, visitingInfo)
    {
    }

    /// <inheritdoc />
    public override char GetDelimiter()
    {
        return '"';
    }

    /// <inheritdoc />
    public override string NewEntityPrefix => ":NEW";
    
    /// <inheritdoc />
    public override string OldEntityPrefix => ":OLD";
    
    /// <inheritdoc />
    public override string GetSql(string source)
    {
        return $"N{Quote}{source}{Quote}";
    }
}