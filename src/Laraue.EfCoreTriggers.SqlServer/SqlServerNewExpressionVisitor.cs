using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.SqlServer;

public class SqlServerNewExpressionVisitor : NewExpressionVisitor
{
    protected override SqlBuilder GetNewGuidSql()
    {
        return SqlBuilder.FromString("NEWID()");
    }

    /// <inheritdoc />
    protected override SqlBuilder GetNewDateTimeOffsetSql()
    {
        return SqlBuilder.FromString("GETDATE()");
    }
}