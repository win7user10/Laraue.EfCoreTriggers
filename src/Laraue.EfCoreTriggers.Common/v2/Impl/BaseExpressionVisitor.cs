using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.v2.Internal;

namespace Laraue.EfCoreTriggers.Common.v2.Impl;

public abstract class BaseExpressionVisitor<TExpression> : IExpressionVisitor<TExpression>
    where TExpression : Expression 
{
    private readonly IExpressionTreeVisitorFactory _factory;

    protected BaseExpressionVisitor(IExpressionTreeVisitorFactory factory)
    {
        _factory = factory;
    }

    public SqlBuilder Visit(Expression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
    {
        var visitor = _factory.GetExpressionTreeVisitor(expression);
        return visitor.Visit(expression, argumentTypes, visitedMembers);
    }

    public abstract SqlBuilder Visit(
        TExpression expression,
        ArgumentTypes argumentTypes,
        VisitedMembers visitedMembers);
}