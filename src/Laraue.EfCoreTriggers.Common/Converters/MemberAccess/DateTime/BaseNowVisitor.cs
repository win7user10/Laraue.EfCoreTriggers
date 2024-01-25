using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MemberAccess.DateTime
{
    /// <summary>
    /// Visitor for <see cref="System.DateTime.Now"/>.
    /// </summary>
    public abstract class BaseNowVisitor : BaseDateTimeVisitor
    {
        /// <inheritdoc />
        protected override string PropertyName => nameof(System.DateTime.Now);

        /// <inheritdoc />
        protected BaseNowVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }
    }
}
