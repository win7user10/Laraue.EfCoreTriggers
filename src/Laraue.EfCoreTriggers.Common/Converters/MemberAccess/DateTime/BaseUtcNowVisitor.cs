using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MemberAccess.DateTime
{
    /// <summary>
    /// Visitor for <see cref="System.DateTime.UtcNow"/>,
    /// </summary>
    public abstract class BaseUtcNowVisitor : BaseDateTimeVisitor
    {
        /// <inheritdoc />
        protected override string PropertyName => nameof(System.DateTime.UtcNow);

        /// <inheritdoc />
        protected BaseUtcNowVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }
    }
}
