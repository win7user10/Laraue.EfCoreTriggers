using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.Common.v2.Internal;

public interface IExpressionVisitor<in TExpression> : IExpressionVisitor 
    where TExpression : Expression
{
    SqlBuilder Visit(TExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers);
}

public interface IExpressionVisitor
{
    SqlBuilder Visit(Expression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers);
}