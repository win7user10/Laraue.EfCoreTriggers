using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.v2.Internal;

namespace Laraue.EfCoreTriggers.Common.v2.Impl;

public class RootExpressionVisitor : IRootExpressionVisitor
{
    public IReadOnlyDictionary<ArgumentType, HashSet<MemberInfo>> VisitedMembers => _visitedMembers;
    private readonly VisitedMembers _visitedMembers = new ();
    private readonly IExpressionTreeVisitorFactory _factory;
    
    public RootExpressionVisitor(IExpressionTreeVisitorFactory factory)
    {
        _factory = factory;
    }

    public SqlBuilder Visit(Expression expression, ArgumentTypes argumentTypes)
    {
        var expressionVisitor = _factory.GetExpressionTreeVisitor(expression);
        return expressionVisitor.Visit(expression, argumentTypes, _visitedMembers);
    }
}