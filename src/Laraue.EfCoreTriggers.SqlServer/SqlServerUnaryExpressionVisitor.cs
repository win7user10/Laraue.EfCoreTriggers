using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.SqlServer;

public class SqlServerUnaryExpressionVisitor : UnaryExpressionVisitor
{
    public SqlServerUnaryExpressionVisitor(
        IExpressionVisitorFactory factory,
        ISqlGenerator generator,
        IDbSchemaRetriever dbSchemaRetriever)
        : base(factory, generator, dbSchemaRetriever)
    {
    }
    
    public override SqlBuilder Visit(UnaryExpression expression, VisitedMembers visitedMembers)
    {
        var unarySql = base.Visit(expression, visitedMembers);
            
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

        // Member expressions are already casted, stay at as is
        if (expression.Operand is MemberExpression)
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