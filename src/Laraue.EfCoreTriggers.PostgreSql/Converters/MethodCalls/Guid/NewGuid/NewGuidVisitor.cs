using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Guid.NewGuid;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.PostgreSql.Converters.MethodCalls.Guid.NewGuid;

/// <inheritdoc />
public class NewGuidVisitor : BaseNewGuidVisitor
{
    public NewGuidVisitor(IExpressionVisitorFactory visitorFactory)
        : base(visitorFactory)
    {
    }

    /// <inheritdoc />
    protected override string NewGuidSql => "gen_random_uuid()";
}