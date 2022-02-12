using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.SetExpressionVisitors;

/// <inheritdoc />
public class SetNewExpressionVisitor : IMemberInfoVisitor<NewExpression>
{
    private readonly IExpressionVisitorFactory _factory;

    /// <summary>
    /// Initializes a new instance of <see cref="SetNewExpressionVisitor"/>.
    /// </summary>
    /// <param name="factory"></param>
    public SetNewExpressionVisitor(IExpressionVisitorFactory factory)
    {
        _factory = factory;
    }

    /// <inheritdoc />
    public Dictionary<MemberInfo, SqlBuilder> Visit(NewExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
    {
        return expression.Arguments.ToDictionary(
            argument => ((MemberExpression)argument).Member,
            argument => _factory.Visit(argument, argumentTypes, visitedMembers));
    }
}