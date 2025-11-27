using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Laraue.Triggers.Core.Converters.MethodCall;
using Laraue.Triggers.Core.SqlGeneration;

namespace Laraue.Triggers.Core.Visitors.ExpressionVisitors
{
    /// <inheritdoc />
    public class MethodCallExpressionVisitor : BaseExpressionVisitor<MethodCallExpression>
    {
        private readonly IMethodCallVisitor[] _visitors;

        /// <inheritdoc />
        public MethodCallExpressionVisitor(IEnumerable<IMethodCallVisitor> methodCallVisitors)
        {
            _visitors = methodCallVisitors.Reverse().ToArray();
        }

        /// <inheritdoc />s
        public override SqlBuilder Visit(MethodCallExpression expression, VisitedMembers visitedMembers)
        {
            var visitor = GetVisitor(expression);
        
            return visitor.Visit(expression, visitedMembers);
        }

        private IMethodCallVisitor GetVisitor(MethodCallExpression expression)
        {
            foreach (var converter in _visitors)
            {
                if (converter.IsApplicable(expression))
                {
                    return converter;
                }
            }
        
            throw new NotSupportedException($"Method {expression.Method.Name} is not supported");
        }
    }
}