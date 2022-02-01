using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall
{
    public abstract class BaseMethodCallVisitor : IMethodCallVisitor
    {
        protected abstract string MethodName { get; }
        protected abstract Type ReflectedType { get; }
        protected IExpressionVisitorFactory VisitorFactory { get; }
        
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
            ArgumentTypes argumentTypes,
            VisitedMembers visitedMembers);
    }
}