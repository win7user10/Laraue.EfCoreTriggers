using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.v2.Internal;

namespace Laraue.EfCoreTriggers.Common.v2.Impl;

public class ConstantExpressionVisitor : BaseExpressionVisitor<ConstantExpression>
{
    private readonly ISqlGenerator _generator;
    
    public ConstantExpressionVisitor(IExpressionTreeVisitorFactory factory, ISqlGenerator generator) 
        : base(factory)
    {
        _generator = generator;
    }

    public override SqlBuilder Visit(ConstantExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
    {
        switch (expression.Value)
        {
            case string strValue:
                return new SqlBuilder(_generator.GetSql(strValue));
            case Enum enumValue:
                return new SqlBuilder(_generator.GetSql(enumValue));
            case bool boolValue:
                return new SqlBuilder(_generator.GetSql(boolValue));
            case null:
                return new SqlBuilder(_generator.GetNullValueSql());
            default:
                return new SqlBuilder(expression.Value.ToString()?.ToLower());
        }
    }
}