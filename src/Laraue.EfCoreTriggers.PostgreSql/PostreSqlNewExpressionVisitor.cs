using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.PostgreSql;

public class PostreSqlNewExpressionVisitor : NewExpressionVisitor
{
    protected override SqlBuilder GetNewGuidSql()
    {
        return SqlBuilder.FromString("uuid_generate_v4()");
    }
}