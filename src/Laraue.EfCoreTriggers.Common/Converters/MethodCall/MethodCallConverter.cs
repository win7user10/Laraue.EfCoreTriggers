using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall
{
    public abstract class MethodCallConverter : IMethodCallConverter
    {
        public abstract string MethodName { get; }
        public abstract  Type ReflectedType { get; }
        /// <inheritdoc />
        public bool IsApplicable(MethodCallExpression expression)
        {
            return expression.Method.ReflectedType == ReflectedType && MethodName == expression.Method.Name;
        }

        /// <inheritdoc />
        public abstract SqlBuilder BuildSql(BaseExpressionProvider provider, MethodCallExpression expression,
            Dictionary<string, ArgumentType> argumentTypes);
    }
}