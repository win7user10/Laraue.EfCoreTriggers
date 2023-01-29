using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;

/// <inheritdoc />
public class ParameterExpressionVisitor : BaseExpressionVisitor<ParameterExpression>
{
    private readonly ISqlGenerator _generator;

    public ParameterExpressionVisitor(ISqlGenerator generator)
    {
        _generator = generator;
    }

    /// <inheritdoc />
    public override SqlBuilder Visit(ParameterExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
    {
        return new SqlBuilder()
            .Append(_generator.GetTableSql(expression.Type));
    }
}