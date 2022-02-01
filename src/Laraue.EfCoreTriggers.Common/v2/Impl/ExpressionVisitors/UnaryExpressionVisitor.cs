using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

public class UnaryExpressionVisitor : BaseExpressionVisitor<UnaryExpression>
{
    private readonly IExpressionVisitorFactory _factory;
    private readonly ISqlGenerator _generator;
    
    public UnaryExpressionVisitor(IExpressionVisitorFactory factory, ISqlGenerator generator)
    {
        _factory = factory;
        _generator = generator;
    }

    public override SqlBuilder Visit(UnaryExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
    {
        var internalExpressionSql = _factory.Visit(expression.Operand, argumentTypes, visitedMembers);
        var sqlBuilder = new SqlBuilder();
        
        if (expression.NodeType == ExpressionType.Convert)
        {
            if (IsNeedConversion(expression))
            {
                sqlBuilder.Append(GetConvertExpressionSql(expression, internalExpressionSql));
            }
            else
            {
                sqlBuilder = internalExpressionSql;
            }

            return sqlBuilder;
        }
            
        var operand = _generator.GetOperand(expression);
        
        var sql = expression.NodeType == ExpressionType.Negate 
            ? $"{operand}{internalExpressionSql}" 
            : $"{internalExpressionSql} {operand}";

        sqlBuilder.Append(sql);
            
        return sqlBuilder;
    }
    
    /// <summary>
    /// Analyze, does passed <see cref="UnaryExpression"/> needs to cast into the Database.
    /// For example, casting of <see cref="Enum"/> values to <see cref="int"/> is not necessary, 
    /// because each <see cref="Enum"></see> is stored as <see cref="int"/> in the Database.
    /// </summary>
    /// <param name="unaryExpression"></param>
    /// <returns></returns>
    protected virtual bool IsNeedConversion(UnaryExpression unaryExpression)
    {
        var clrType1 = unaryExpression.Operand.Type;
        var clrType2 = unaryExpression.Type;
        if (clrType1 == typeof(object) || clrType2 == typeof(object))
        {
            return false;
        }
        
        var sqlType1 = _generator.GetSqlType(clrType1);
        var sqlType2 = _generator.GetSqlType(clrType2);
        return sqlType1 != sqlType2;
    }
    
    /// <summary>
    /// Generate SQL expression to cast passed <paramref name="member"/>
    /// to type represents in <paramref name="unaryExpression"/>.
    /// </summary>
    /// <param name="unaryExpression"></param>
    /// <param name="member"></param>
    /// <returns></returns>
    protected virtual string GetConvertExpressionSql(UnaryExpression unaryExpression, string member)
    {
        var sqlType = _generator.GetSqlType(unaryExpression.Type);
        return sqlType is not null
            ? $"CAST({member} AS {sqlType})"
            : throw new NotSupportedException($"Converting of type {unaryExpression.Type} is not supported");
    }
}