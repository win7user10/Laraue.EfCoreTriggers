﻿using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;

/// <inheritdoc />
public class ExpressionVisitorFactory : IExpressionVisitorFactory
{
    private readonly IServiceProvider _provider;

    /// <summary>
    /// Initializes a new instance of <see cref="ExpressionVisitorFactory"/>.
    /// </summary>
    /// <param name="provider"></param>
    public ExpressionVisitorFactory(IServiceProvider provider)
    {
        _provider = provider;
    }
    
    /// <inheritdoc />
    public SqlBuilder Visit(Expression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
    {
        return expression switch
        {
            BinaryExpression binary => Visit(binary, argumentTypes, visitedMembers),
            ConstantExpression constant => Visit(constant, argumentTypes, visitedMembers),
            MemberExpression member => Visit(member, argumentTypes, visitedMembers),
            MethodCallExpression methodCall => Visit(methodCall, argumentTypes, visitedMembers),
            UnaryExpression unary => Visit(unary, argumentTypes, visitedMembers),
            NewExpression @new => Visit(@new, argumentTypes, visitedMembers),
            _ => throw new NotSupportedException($"Expression of type {expression.GetType()} is not supported")
        };
    }

    private SqlBuilder Visit<TExpression>(TExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
        where TExpression : Expression
    {
        var visitor = _provider.GetService<IExpressionVisitor<TExpression>>();

        if (visitor is null)
        {
            throw new NotSupportedException("Not supported " + typeof(TExpression));
        }
        
        return visitor.Visit(expression, argumentTypes, visitedMembers);
    }
}