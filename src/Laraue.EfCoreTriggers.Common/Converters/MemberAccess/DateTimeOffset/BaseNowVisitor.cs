using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MemberAccess.DateTimeOffset
{
    /// <summary>
    /// Visitor for <see cref="System.DateTime.Now"/>.
    /// </summary>
    public abstract class BaseNowVisitor : BaseDateTimeOffsetVisitor
    {
        /// <inheritdoc />
        protected override string PropertyName => nameof(System.DateTimeOffset.Now);

        /// <inheritdoc />
        protected BaseNowVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }
    }
}
