using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.Common.v2.Impl.SetExpressionVisitors;

public class SetLambdaExpressionVisitor : ISetExpressionVisitor<LambdaExpression>
{
    private readonly ISetExpressionVisitorFactory _factory;

    public SetLambdaExpressionVisitor(ISetExpressionVisitorFactory factory)
    {
        _factory = factory;
    }

    public Dictionary<MemberInfo, SqlBuilder> Visit(LambdaExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
    {
        return _factory.Visit(expression.Body, argumentTypes, visitedMembers);
    }
}