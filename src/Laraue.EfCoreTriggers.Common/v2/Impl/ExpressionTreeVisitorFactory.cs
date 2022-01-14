using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.v2.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.Common.v2.Impl;

public class ExpressionTreeVisitorFactory : IExpressionTreeVisitorFactory
{
    private readonly IServiceProvider _provider;

    public ExpressionTreeVisitorFactory(IServiceProvider provider)
    {
        _provider = provider;
    }
    
    public IExpressionVisitor<Expression> GetExpressionTreeVisitor(Expression expression)
    {
        return expression switch
        {
            BinaryExpression => GetExpressionTreeVisitor<BinaryExpression>(),
            MemberExpression => GetExpressionTreeVisitor<MemberExpression>(),
            UnaryExpression => GetExpressionTreeVisitor<UnaryExpression>(),
            NewExpression => GetExpressionTreeVisitor<NewExpression>(),
            ConstantExpression => GetExpressionTreeVisitor<ConstantExpression>(),
            MethodCallExpression => GetExpressionTreeVisitor<MethodCallExpression>(),
            null => throw new ArgumentNullException(nameof(expression)),
            _ => throw new NotSupportedException($"Expression of type {expression.GetType()} for {expression} is not supported."),
        };
    }

    private IExpressionVisitor<Expression> GetExpressionTreeVisitor<TExpression>()
        where TExpression : Expression
    {
        return (IExpressionVisitor<Expression>) _provider.GetRequiredService<IExpressionVisitor<TExpression>>();
    }
}