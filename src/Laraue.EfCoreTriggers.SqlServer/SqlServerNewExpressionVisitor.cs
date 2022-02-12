using Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.SqlServer;

public class SqlServerNewExpressionVisitor : NewExpressionVisitor
{
    protected override SqlBuilder GetNewGuidSql()
    {
        return SqlBuilder.FromString("NEWID()");
    }
}