using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;

/// <inheritdoc />
public abstract class NewExpressionVisitor : BaseExpressionVisitor<NewExpression>
{
    /// <inheritdoc />
    public override SqlBuilder Visit(NewExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
    {
        if (expression.Type == typeof(Guid))
        {
            return GetNewGuidSql();
        }
        
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Generate new Guid SQL.
    /// </summary>
    /// <returns></returns>
    protected abstract SqlBuilder GetNewGuidSql();
}