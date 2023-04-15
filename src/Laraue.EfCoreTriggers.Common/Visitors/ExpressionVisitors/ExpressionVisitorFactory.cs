using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors
{
    /// <inheritdoc />
    public class ExpressionVisitorFactory : IExpressionVisitorFactory
    {
        private readonly IServiceProvider _provider;
        private readonly VisitingInfo _visitingInfo;

        /// <summary>
        /// Initializes a new instance of <see cref="ExpressionVisitorFactory"/>.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="visitingInfo"></param>
        public ExpressionVisitorFactory(IServiceProvider provider, VisitingInfo visitingInfo)
        {
            _provider = provider;
            _visitingInfo = visitingInfo;
        }
    
        /// <inheritdoc />
        public SqlBuilder Visit(Expression expression, VisitedMembers visitedMembers)
        {
            return expression switch
            {
                BinaryExpression binary => Visit(binary, visitedMembers),
                ConstantExpression constant => Visit(constant, visitedMembers),
                MemberExpression member => VisitAndRememberMember(member, visitedMembers),
                MethodCallExpression methodCall => Visit(methodCall, visitedMembers),
                UnaryExpression unary => Visit(unary, visitedMembers),
                NewExpression @new => Visit(@new, visitedMembers),
                LambdaExpression lambda => Visit(lambda, visitedMembers),
                ParameterExpression parameterExpression => Visit(parameterExpression, visitedMembers),
                _ => throw new NotSupportedException($"Expression of type {expression.GetType()} is not supported")
            };
        }
    
        private SqlBuilder VisitAndRememberMember(MemberExpression expression, VisitedMembers visitedMembers)
        {
            return _visitingInfo.ExecuteWithChangingMember(
                expression.Member,
                () => Visit(expression, visitedMembers));
        }

        private SqlBuilder Visit<TExpression>(TExpression expression, VisitedMembers visitedMembers)
            where TExpression : Expression
        {
            var visitor = GetExpressionVisitor<TExpression>();
        
            return visitor.Visit(expression, visitedMembers);
        }
    
        public IExpressionVisitor<TExpression> GetExpressionVisitor<TExpression>() 
            where TExpression : Expression
        {
            var visitor = _provider.GetService<IExpressionVisitor<TExpression>>();

            if (visitor is null)
            {
                throw new NotSupportedException("Not supported " + typeof(TExpression));
            }

            return visitor;
        }
    }
}