using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.Visitors.SetExpressionVisitors;
using Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors.Statements;

namespace Laraue.EfCoreTriggers.MySql;

/// <inheritdoc />
public sealed class MySqlInsertExpressionVisitor : InsertExpressionVisitor
{
    /// <inheritdoc />
    public MySqlInsertExpressionVisitor(IMemberInfoVisitorFactory factory, ISqlGenerator sqlGenerator) 
        : base(factory, sqlGenerator)
    {
    }

    /// <inheritdoc />
    protected override SqlBuilder VisitEmptyInsertBody(LambdaExpression insertExpression)
    {
        var sqlBuilder = new SqlBuilder();
        sqlBuilder.Append("() VALUES ()");
        return sqlBuilder;
    }
}