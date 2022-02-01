using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.Common.v2.Impl.SetExpressionVisitors;

public interface ISetExpressionVisitorFactory
{
    Dictionary<MemberInfo, SqlBuilder> Visit(Expression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers);
}

public class SetExpressionVisitorFactory : ISetExpressionVisitorFactory
{
    private readonly IServiceProvider _serviceProvider;

    public SetExpressionVisitorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Dictionary<MemberInfo, SqlBuilder> Visit(Expression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
    {
        return expression switch
        {
            LambdaExpression lambdaExpression => Visit(lambdaExpression, argumentTypes, visitedMembers),
            MemberInitExpression memberInitExpression => Visit(memberInitExpression, argumentTypes, visitedMembers),
            NewExpression newExpression => Visit(newExpression, argumentTypes, visitedMembers),
            _ => throw new NotSupportedException($"Expression of type {expression.GetType()} is not supported")
        };
    }
    
    private Dictionary<MemberInfo, SqlBuilder> Visit<TExpression>(TExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
        where TExpression : Expression
    {
        return _serviceProvider.GetRequiredService<ISetExpressionVisitor<TExpression>>()
            .Visit(expression, argumentTypes, visitedMembers);
    }
}