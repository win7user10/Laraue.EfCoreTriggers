using Laraue.Linq2Triggers.Core.Converters.MethodCall.Guid.NewGuid;
using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Linq2Triggers.Providers.MySql.Converters.MethodCalls.Guid.NewGuid;

/// <inheritdoc />
public class NewGuidVisitor : BaseNewGuidVisitor
{
    public NewGuidVisitor(IExpressionVisitorFactory visitorFactory)
        : base(visitorFactory)
    {
    }

    /// <inheritdoc />
    protected override string NewGuidSql => "UUID()";
}