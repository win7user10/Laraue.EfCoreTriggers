using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.Common.Visitors.SetExpressionVisitors
{
    /// <summary>
    /// Visitor returns suitable <see cref="IMemberInfoVisitor{TExpression}"/>
    /// depending on passed <see cref="Expression"/> type.
    /// </summary>
    public interface IMemberInfoVisitorFactory
    {
        /// <summary>
        /// Takes suitable <see cref="IMemberInfoVisitor{TExpression}"/>
        /// and calls it <see cref="IMemberInfoVisitor{TExpression}.Visit"/> method.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="visitedMembers"></param>
        /// <returns></returns>
        Dictionary<MemberInfo, SqlBuilder> Visit(
            Expression expression,
            VisitedMembers visitedMembers);
    }
}