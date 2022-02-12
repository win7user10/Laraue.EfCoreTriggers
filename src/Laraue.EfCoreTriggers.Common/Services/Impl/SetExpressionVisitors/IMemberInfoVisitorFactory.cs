using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.SetExpressionVisitors;

/// <summary>
/// Visitor returns suitable <see cref="IMemberInfoVisitor{TExpression}"/>
/// depending on passed <see cref="Expression"/> type.
/// </summary>
public interface IMemberInfoVisitorFactory
{
    /// <summary>
    /// Takes suitable <see cref="IMemberInfoVisitor{TExpression}"/>
    /// and calls it <see cref="IMemberInfoVisitor{TExpression}.Visit"/> method.
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="argumentTypes"></param>
    /// <param name="visitedMembers"></param>
    /// <returns></returns>
    Dictionary<MemberInfo, SqlBuilder> Visit(
        Expression expression, 
        ArgumentTypes argumentTypes, 
        VisitedMembers visitedMembers);
}