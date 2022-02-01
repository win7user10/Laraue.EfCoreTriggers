using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.v2.Impl.SetExpressionVisitors;

public class SetNewExpressionVisitor : ISetExpressionVisitor<NewExpression>
{
    private readonly IExpressionVisitorFactory _factory;

    public SetNewExpressionVisitor(IExpressionVisitorFactory factory)
    {
        _factory = factory;
    }

    public Dictionary<MemberInfo, SqlBuilder> Visit(NewExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
    {
        return expression.Arguments.ToDictionary(
            argument => ((MemberExpression)argument).Member,
            argument => _factory.Visit(argument, argumentTypes, visitedMembers));
    }
}