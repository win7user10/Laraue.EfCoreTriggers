using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Internal;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall
{
    public abstract class MethodCallConverter : IMethodCallConverter
    {
        protected abstract string MethodName { get; }
        protected abstract Type ReflectedType { get; }
        
        /// <inheritdoc />
        public bool IsApplicable(MethodCallExpression expression)
        {
            return expression.Method.ReflectedType == ReflectedType && MethodName == expression.Method.Name;
        }

        /// <inheritdoc />
        public abstract SqlBuilder BuildSql(
            IExpressionVisitor visitor,
            MethodCallExpression expression,
            ArgumentTypes argumentTypes,
            VisitedMembers visitedMembers);
    }
}