using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.SqlLite;

public class SqliteNewExpressionVisitor : NewExpressionVisitor
{
    private readonly ISqlGenerator _sqlGenerator;

    public SqliteNewExpressionVisitor(ISqlGenerator sqlGenerator)
    {
        _sqlGenerator = sqlGenerator;
    }
    
    protected override SqlBuilder GetNewGuidSql()
    {
        return SqlBuilder.FromString(
            "lower(hex(randomblob(4))) || '-' || lower(hex(randomblob(2))) || '-4' || " +
            "substr(lower(hex(randomblob(2))),2) || '-' || substr('89ab', abs(random()) % 4 + 1, 1) || " +
            "substr(lower(hex(randomblob(2))),2) || '-' || lower(hex(randomblob(6)))");
    }

    /// <inheritdoc />
    protected override SqlBuilder GetNewDateTimeOffsetSql()
    {
        var delimiter = _sqlGenerator.GetDelimiter();
        
        return SqlBuilder.FromString($"DATE({delimiter}now{delimiter})");
    }
}