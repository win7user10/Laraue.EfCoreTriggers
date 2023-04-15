using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.Common.Visitors.SetExpressionVisitors
{
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
        public Dictionary<MemberInfo, SqlBuilder> Visit(Expression expression, VisitedMembers visitedMembers)
        {
            return expression switch
            {
                LambdaExpression lambdaExpression => Visit(lambdaExpression, visitedMembers),
                MemberInitExpression memberInitExpression => Visit(memberInitExpression, visitedMembers),
                NewExpression newExpression => Visit(newExpression, visitedMembers),
                BinaryExpression binaryExpression => Visit(binaryExpression, visitedMembers),
                _ => throw new NotSupportedException($"Expression of type {expression.GetType()} is not supported")
            };
        }
    
        private Dictionary<MemberInfo, SqlBuilder> Visit<TExpression>(TExpression expression, VisitedMembers visitedMembers)
            where TExpression : Expression
        {
            return _serviceProvider.GetRequiredService<IMemberInfoVisitor<TExpression>>()
                .Visit(expression, visitedMembers);
        }
    }
}