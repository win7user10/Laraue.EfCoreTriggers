using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.v2.Internal;

namespace Laraue.EfCoreTriggers.Common.v2.Impl;

public class MemberExpressionVisitor : BaseExpressionVisitor<MemberExpression>
{
    private readonly ISqlGenerator _generator;
    
    public MemberExpressionVisitor(IExpressionTreeVisitorFactory factory) : base(factory)
    {
    }

    public override SqlBuilder Visit(MemberExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
    {
        argumentTypes ??= new ArgumentTypes();
        var parameterExpression = (ParameterExpression)expression.Expression;
        var memberName = parameterExpression.Name;
        if (!argumentTypes.TryGetValue(memberName, out var argumentType))
            argumentType = ArgumentType.Default;
        return new SqlBuilder(expression.Member, argumentType)
            .Append(Visit(expression, argumentType));
    }
    
    protected virtual string Visit(MemberExpression memberExpression, ArgumentType argumentType)
    {
        return _generator.GetColumnSql(memberExpression.Member, argumentType);
    }
}