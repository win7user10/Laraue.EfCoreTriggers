using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

public abstract class NewExpressionVisitor : BaseExpressionVisitor<NewExpression>
{
    public override SqlBuilder Visit(NewExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
    {
        if (expression.Type == typeof(Guid))
        {
            return GetNewGuidSql();
        }
        
        throw new System.NotImplementedException();
    }

    protected abstract SqlBuilder GetNewGuidSql();
}