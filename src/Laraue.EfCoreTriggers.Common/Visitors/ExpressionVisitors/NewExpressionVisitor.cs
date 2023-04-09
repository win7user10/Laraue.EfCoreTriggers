using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors
{
    /// <inheritdoc />
    public abstract class NewExpressionVisitor : BaseExpressionVisitor<NewExpression>
    {
        /// <inheritdoc />
        public override SqlBuilder Visit(NewExpression expression, VisitedMembers visitedMembers)
        {
            if (expression.Type == typeof(Guid))
            {
                return GetNewGuidSql();
            }
        
            if (expression.Type == typeof(DateTimeOffset))
            {
                return GetNewDateTimeOffsetSql();
            }
        
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generate new Guid SQL.
        /// </summary>
        /// <returns></returns>
        protected abstract SqlBuilder GetNewGuidSql();
    
        /// <summary>
        /// Generate new DateTimeOffset SQL.
        /// </summary>
        /// <returns></returns>
        protected abstract SqlBuilder GetNewDateTimeOffsetSql();
    }
}