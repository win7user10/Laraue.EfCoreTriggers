﻿using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;

/// <inheritdoc />
public class ConstantExpressionVisitor : BaseExpressionVisitor<ConstantExpression>
{
    private readonly ISqlGenerator _generator;
    
    /// <inheritdoc />
    public ConstantExpressionVisitor(ISqlGenerator generator)
    {
        _generator = generator;
    }

    /// <inheritdoc />
    public override SqlBuilder Visit(ConstantExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
    {
        switch (expression.Value)
        {
            case string strValue:
                return SqlBuilder.FromString(_generator.GetSql(strValue));
            case Enum enumValue:
                return SqlBuilder.FromString(_generator.GetSql(enumValue));
            case bool boolValue:
                return SqlBuilder.FromString(_generator.GetSql(boolValue));
            case null:
                return SqlBuilder.FromString(_generator.GetNullValueSql());
            default:
                return SqlBuilder.FromString(expression.Value.ToString()?.ToLower());
        }
    }
}