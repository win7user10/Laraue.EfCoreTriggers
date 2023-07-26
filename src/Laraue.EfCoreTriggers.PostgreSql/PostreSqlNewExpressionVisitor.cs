using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.PostgreSql;

public class PostreSqlNewExpressionVisitor : NewExpressionVisitor
{
    protected override SqlBuilder GetNewGuidSql()
    {
        return SqlBuilder.FromString("gen_random_uuid()");
    }

    /// <inheritdoc />
    protected override SqlBuilder GetNewDateTimeOffsetSql()
    {
        return SqlBuilder.FromString("CURRENT_TIMESTAMP");
    }
}