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
public sealed class GetTableNameVisitor : BaseTriggerFunctionsVisitor
{
    private readonly ISqlGenerator _sqlGenerator;

    /// Initializes a new instance of <see cref="GetTableNameVisitor"/>.
    public GetTableNameVisitor(IExpressionVisitorFactory visitorFactory, ISqlGenerator sqlGenerator)
        : base(visitorFactory)
    {
        _sqlGenerator = sqlGenerator;
    }

    /// <inheritdoc />
    protected override string MethodName => nameof(TriggerFunctions.GetTableName);
    
    /// <inheritdoc />
    public override SqlBuilder Visit(
        MethodCallExpression expression,
        ArgumentTypes argumentTypes,
        VisitedMembers visitedMembers)
    {
        var entityType = expression.Method.GetGenericArguments()[0];

        return new SqlBuilder().Append(_sqlGenerator.GetTableSql(entityType));
    }
}