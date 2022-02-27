using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.SetExpressionVisitors;

/// <inheritdoc />
public class MemberInfoVisitorFactory : IMemberInfoVisitorFactory
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of <see cref="MemberInfoVisitorFactory"/>.
    /// </summary>
    /// <param name="serviceProvider"></param>
    public MemberInfoVisitorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public Dictionary<MemberInfo, SqlBuilder> Visit(Expression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
    {
        return expression switch
        {
            LambdaExpression lambdaExpression => Visit(lambdaExpression, argumentTypes, visitedMembers),
            MemberInitExpression memberInitExpression => Visit(memberInitExpression, argumentTypes, visitedMembers),
            NewExpression newExpression => Visit(newExpression, argumentTypes, visitedMembers),
            BinaryExpression binaryExpression => Visit(binaryExpression, argumentTypes, visitedMembers),
            _ => throw new NotSupportedException($"Expression of type {expression.GetType()} is not supported")
        };
    }
    
    private Dictionary<MemberInfo, SqlBuilder> Visit<TExpression>(TExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
        where TExpression : Expression
    {
        return _serviceProvider.GetRequiredService<IMemberInfoVisitor<TExpression>>()
            .Visit(expression, argumentTypes, visitedMembers);
    }
}