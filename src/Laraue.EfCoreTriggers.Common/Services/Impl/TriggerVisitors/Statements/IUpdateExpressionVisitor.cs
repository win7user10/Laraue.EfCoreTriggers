using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors.Statements;

/// <summary>
/// Generates SQL for update SQL statement.
/// </summary>
public interface IUpdateExpressionVisitor
{
    /// <summary>
    /// Generates update SQL for the passed <see cref="LambdaExpression"/> without "UPDATE" keyword,
    /// e.g. SET column1 = "value1", column2 = "value2"
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="argumentTypes"></param>
    /// <param name="visitedMembers"></param>
    /// <returns></returns>
    SqlBuilder Visit(LambdaExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers);
}