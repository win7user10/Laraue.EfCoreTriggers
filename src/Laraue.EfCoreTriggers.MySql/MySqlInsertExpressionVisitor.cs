using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.Services.Impl.SetExpressionVisitors;
using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors.Statements;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.MySql;

public class MySqlInsertExpressionVisitor : InsertExpressionVisitor
{
    public MySqlInsertExpressionVisitor(IMemberInfoVisitorFactory factory, IDbSchemaRetriever adapter, ISqlGenerator sqlGenerator) 
        : base(factory, adapter, sqlGenerator)
    {
    }

    protected override SqlBuilder VisitEmptyInsertBody(LambdaExpression insertExpression, ArgumentTypes argumentTypes)
    {
        var sqlBuilder = new SqlBuilder();
        sqlBuilder.Append("() VALUES ()");
        return sqlBuilder;
    }
}