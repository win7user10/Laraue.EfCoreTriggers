using Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.MySql;

public class MySqlNewExpressionVisitor : NewExpressionVisitor
{
    protected override SqlBuilder GetNewGuidSql()
    {
        return SqlBuilder.FromString("UUID()");
    }
}