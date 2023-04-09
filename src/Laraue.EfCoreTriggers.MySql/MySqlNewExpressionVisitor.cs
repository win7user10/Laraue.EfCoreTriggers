using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.MySql;

public class MySqlNewExpressionVisitor : NewExpressionVisitor
{
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