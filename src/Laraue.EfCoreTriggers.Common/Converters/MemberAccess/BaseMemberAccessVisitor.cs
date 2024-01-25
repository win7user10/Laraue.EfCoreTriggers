using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MemberAccess
{
    /// <summary>
    /// Base <see cref="IMethodCallVisitor"/>.
    /// </summary>
    public abstract class BaseMemberAccessVisitor : IMemberAccessVisitor
    {
        /// <summary>
        /// Specifies a method which will be handled by this converter.
        /// </summary>
        protected abstract string PropertyName { get; }
        
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
        protected BaseMemberAccessVisitor(IExpressionVisitorFactory visitorFactory)
        {
            VisitorFactory = visitorFactory;
        }
        
        /// <inheritdoc />
        public bool IsApplicable(MemberExpression expression)
        {
            return expression.Member.ReflectedType == ReflectedType && PropertyName == expression.Member.Name;
        }

        /// <inheritdoc />
        public abstract SqlBuilder Visit(MemberExpression expression);
    }
}