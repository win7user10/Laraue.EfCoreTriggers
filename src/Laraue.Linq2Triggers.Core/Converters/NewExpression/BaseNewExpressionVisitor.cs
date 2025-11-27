using System;
using Laraue.Linq2Triggers.Core.Converters.MethodCall;
using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Linq2Triggers.Core.Converters.NewExpression
{
    /// <summary>
    /// Base <see cref="IMethodCallVisitor"/>.
    /// </summary>
    public abstract class BaseNewExpressionVisitor : INewExpressionVisitor
    {
        /// <summary>
        /// Specifies a class which construcors will be handled by this converter.
        /// </summary>
        protected abstract Type ReflectedType { get; }
        
        /// <summary>
        /// Factory to visit expressions and generate SQL code.
        /// </summary>
        protected IExpressionVisitorFactory VisitorFactory { get; }
        
        /// <summary>
        /// Initialize a new instance of <see cref="BaseNewExpressionVisitor"/>.
        /// </summary>
        /// <param name="visitorFactory"></param>
        protected BaseNewExpressionVisitor(IExpressionVisitorFactory visitorFactory)
        {
            VisitorFactory = visitorFactory;
        }
        
        /// <inheritdoc />
        public bool IsApplicable(System.Linq.Expressions.NewExpression expression)
        {
            return expression.Type == ReflectedType;
        }

        /// <inheritdoc />
        public abstract SqlBuilder Visit(
            System.Linq.Expressions.NewExpression expression,
            VisitedMembers visitedMembers);
    }
}