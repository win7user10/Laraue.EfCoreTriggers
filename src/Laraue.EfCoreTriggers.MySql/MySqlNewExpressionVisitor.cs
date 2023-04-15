using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.MySql;

/// <inheritdoc />
public sealed class MySqlNewExpressionVisitor : NewExpressionVisitor
{
    /// <inheritdoc />
    protected override SqlBuilder GetNewGuidSql()
    {
        return SqlBuilder.FromString("UUID()");
    }

    /// <inheritdoc />
    protected override SqlBuilder GetNewDateTimeOffsetSql()
    {
        return SqlBuilder.FromString("CURRENT_DATE()");
    }
}