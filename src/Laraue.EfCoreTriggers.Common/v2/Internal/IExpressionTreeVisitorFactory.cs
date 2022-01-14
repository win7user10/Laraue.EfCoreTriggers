using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.v2.Internal;

public interface IExpressionTreeVisitorFactory
{
    IExpressionVisitor<Expression> GetExpressionTreeVisitor(Expression expression);
}