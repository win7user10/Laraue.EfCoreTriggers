﻿using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.SetExpressionVisitors;

/// <summary>
/// Visitor returns <see cref="SqlBuilder"/> for each member
/// without combine it in the one SQL.
/// </summary>
/// <typeparam name="TExpression"></typeparam>
public interface IMemberInfoVisitor<in TExpression>
    where TExpression : Expression
{
    /// <summary>
    /// Visit passed <see cref="Expression"/> and return
    /// SQL for each of it members.
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="argumentTypes"></param>
    /// <param name="visitedMembers"></param>
    /// <returns></returns>
    Dictionary<MemberInfo, SqlBuilder> Visit(
        TExpression expression,
        ArgumentTypes argumentTypes,
        VisitedMembers visitedMembers);
}