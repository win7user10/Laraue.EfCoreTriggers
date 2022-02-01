using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

public interface IExpressionVisitorFactory
{
    SqlBuilder Visit(
        Expression expression,
        ArgumentTypes argumentTypes,
        VisitedMembers visitedMembers);
}