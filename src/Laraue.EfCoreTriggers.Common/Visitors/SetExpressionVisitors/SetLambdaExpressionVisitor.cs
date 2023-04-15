using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.Common.Visitors.SetExpressionVisitors
{
    /// <inheritdoc />
    public class SetLambdaExpressionVisitor : IMemberInfoVisitor<LambdaExpression>
    {
        private readonly IMemberInfoVisitorFactory _factory;
    
        /// <summary>
        /// Initializes a new instance of <see cref="SetLambdaExpressionVisitor"/>.
        /// </summary>
        /// <param name="factory"></param>
        public SetLambdaExpressionVisitor(IMemberInfoVisitorFactory factory)
        {
            _factory = factory;
        }

        /// <inheritdoc />
        public Dictionary<MemberInfo, SqlBuilder> Visit(LambdaExpression expression, VisitedMembers visitedMembers)
        {
            return _factory.Visit(expression.Body, visitedMembers);
        }
    }
}