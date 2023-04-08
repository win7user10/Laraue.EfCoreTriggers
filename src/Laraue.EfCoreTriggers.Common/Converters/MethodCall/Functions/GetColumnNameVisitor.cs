using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Functions;
using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Functions;

/// <summary>
/// Translates <see cref="TriggerFunctions"/> methods SQL.
/// </summary>
public sealed class GetColumnNameVisitor : BaseTriggerFunctionsVisitor
{
    /// Initializes a new instance of <see cref="GetColumnNameVisitor"/>.
    public GetColumnNameVisitor(IExpressionVisitorFactory visitorFactory)
        : base(visitorFactory)
    {
    }

    /// <inheritdoc />
    protected override string MethodName => nameof(TriggerFunctions.GetColumnName);
    
    /// <inheritdoc />
    public override SqlBuilder Visit(
        MethodCallExpression expression,
        ArgumentTypes argumentTypes,
        VisitedMembers visitedMembers)
    {
        return VisitorFactory.Visit(expression.Arguments[0], argumentTypes, visitedMembers);
    }
}