using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common;
using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.SqlServer;

public class SqlServerUnaryExpressionVisitor : UnaryExpressionVisitor
{
    private readonly ISqlGenerator _sqlGenerator;
    
    public SqlServerUnaryExpressionVisitor(
        IExpressionVisitorFactory factory, 
        ISqlGenerator sqlGenerator) 
        : base(factory, sqlGenerator)
    {
        _sqlGenerator = sqlGenerator;
    }

    public override SqlBuilder Visit(UnaryExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
    {
        var unarySql = base.Visit(expression, argumentTypes, visitedMembers);
            
        if (ShouldBeCastedToBoolean(expression))
        {
            unarySql.Prepend("CASE WHEN ")
                .Append(" THEN 1 ELSE 0 END");
        }

        return unarySql;
    }
    
    private bool ShouldBeCastedToBoolean(UnaryExpression expression)
    {
        if (WasAnyChildCastedToBoolean(expression))
        {
            return false;
        }
            
        if (BooleanExpressionTypes.Contains(expression.NodeType))
        {
            return true;
        }

        return expression.NodeType is ExpressionType.Convert && 
            EfCoreTriggersHelper.GetNotNullableType(expression.Operand.Type) == typeof(bool);
    }
    
    private static readonly HashSet<ExpressionType> BooleanExpressionTypes = new ()
    {
        ExpressionType.Not,
        ExpressionType.Equal
    };

    private bool WasAnyChildCastedToBoolean(UnaryExpression unaryExpression)
    {
        return unaryExpression.Operand is UnaryExpression innerUnaryExpression &&
            ShouldBeCastedToBoolean(innerUnaryExpression);
    }
}