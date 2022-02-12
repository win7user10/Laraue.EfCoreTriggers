﻿using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors.Statements;

/// <summary>
/// Generates SQL for insert SQL statement.
/// </summary>
public interface IInsertExpressionVisitor
{
    /// <summary>
    /// Generates insert SQL for the passed <see cref="LambdaExpression"/> without "INSERT" keyword,
    /// e.g. (column1, column2) VALUES ("value1", "value2")
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="argumentTypes"></param>
    /// <param name="visitedMembers"></param>
    /// <returns></returns>
    SqlBuilder Visit(LambdaExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers);
}