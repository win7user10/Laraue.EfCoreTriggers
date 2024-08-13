using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MemberAccess.DateTimeOffset
{
    /// <summary>
    /// Visitor for <see cref="System.DateTime.UtcNow"/>,
    /// </summary>
    public abstract class BaseUtcNowVisitor : BaseDateTimeOffsetVisitor
    {
        /// <inheritdoc />
        protected override string PropertyName => nameof(System.DateTimeOffset.UtcNow);

        /// <inheritdoc />
        protected BaseUtcNowVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }
    }
}
