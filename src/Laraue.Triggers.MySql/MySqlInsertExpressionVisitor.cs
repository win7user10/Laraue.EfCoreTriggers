using System.Linq.Expressions;
using Laraue.Triggers.Core.SqlGeneration;
using Laraue.Triggers.Core.Visitors.SetExpressionVisitors;
using Laraue.Triggers.Core.Visitors.TriggerVisitors.Statements;

namespace Laraue.Triggers.MySql;

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