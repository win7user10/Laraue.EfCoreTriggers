using System.Linq.Expressions;
using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.Visitors.SetExpressionVisitors;
using Laraue.Linq2Triggers.Core.Visitors.TriggerVisitors.Statements;

namespace Laraue.Linq2Triggers.Providers.Oracle;

/// <inheritdoc />
public sealed class OracleInsertExpressionVisitor : InsertExpressionVisitor
{
    /// <inheritdoc />
    public OracleInsertExpressionVisitor(IMemberInfoVisitorFactory factory, ISqlGenerator sqlGenerator) 
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