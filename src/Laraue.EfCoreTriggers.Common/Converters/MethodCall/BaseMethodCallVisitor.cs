using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall
{
    /// <summary>
    /// Base <see cref="IMethodCallVisitor"/>.
    /// </summary>
    public abstract class BaseMethodCallVisitor : IMethodCallVisitor
    {
        /// <summary>
        /// Specifies a method which will be handled by this converter.
        /// </summary>
        protected abstract string MethodName { get; }
        
        /// <summary>
        /// Specifies a class which methods will be handled by this converter.
        /// </summary>
        protected abstract Type ReflectedType { get; }
        
        /// <summary>
        /// Factory to visit expressions and generate SQL code.
        /// </summary>
        protected IExpressionVisitorFactory VisitorFactory { get; }
        
        /// <summary>
        /// Initialize a new instance of <see cref="BaseMethodCallVisitor"/>.
        /// </summary>
        /// <param name="visitorFactory"></param>
        protected BaseMethodCallVisitor(IExpressionVisitorFactory visitorFactory)
        {
            VisitorFactory = visitorFactory;
        }
        
        /// <inheritdoc />
        public bool IsApplicable(MethodCallExpression expression)
        {
            return expression.Method.ReflectedType == ReflectedType && MethodName == expression.Method.Name;
        }

        /// <inheritdoc />
        public abstract SqlBuilder Visit(
            MethodCallExpression expression,
            VisitedMembers visitedMembers);
    }
}