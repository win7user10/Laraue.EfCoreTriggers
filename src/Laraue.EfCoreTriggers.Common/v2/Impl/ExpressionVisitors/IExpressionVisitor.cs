using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

public interface IExpressionVisitor<in TExpression> where TExpression : Expression
{
    SqlBuilder Visit(TExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers);
}