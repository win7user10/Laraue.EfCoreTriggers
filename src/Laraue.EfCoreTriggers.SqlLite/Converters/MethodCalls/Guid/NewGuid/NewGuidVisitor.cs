using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Guid.NewGuid;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.SqlLite.Converters.MethodCalls.Guid.NewGuid;

/// <inheritdoc />
public class NewGuidVisitor : BaseNewGuidVisitor
{
    public NewGuidVisitor(IExpressionVisitorFactory visitorFactory)
        : base(visitorFactory)
    {
    }

    /// <inheritdoc />
    protected override string NewGuidSql =>
        "lower(hex(randomblob(4))) || '-' || lower(hex(randomblob(2))) || '-4' || " +
        "substr(lower(hex(randomblob(2))),2) || '-' || substr('89ab', abs(random()) % 4 + 1, 1) || " +
        "substr(lower(hex(randomblob(2))),2) || '-' || lower(hex(randomblob(6)))";
}