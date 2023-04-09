using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.Common.Visitors.SetExpressionVisitors
{
    /// <summary>
    /// Visitor returns <see cref="SqlBuilder"/> for each member
    /// without combine it in the one SQL.
    /// </summary>
    /// <typeparam name="TExpression"></typeparam>
    public interface IMemberInfoVisitor<in TExpression>
        where TExpression : Expression
    {
        /// <summary>
        /// Visit passed <see cref="Expression"/> and return
        /// SQL for each of it members.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="visitedMembers"></param>
        /// <returns></returns>
        Dictionary<MemberInfo, SqlBuilder> Visit(
            TExpression expression,
            VisitedMembers visitedMembers);
    }
}