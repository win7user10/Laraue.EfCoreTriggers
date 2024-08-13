using System;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MemberAccess.DateTimeOffset
{
    /// <summary>
    /// Base visitor for <see cref="System.Math"/> methods.
    /// </summary>
    public abstract class BaseDateTimeOffsetVisitor : BaseMemberAccessVisitor
    {
        /// <inheritdoc />
        protected override Type ReflectedType => typeof(System.DateTimeOffset);

        /// <inheritdoc />
        protected BaseDateTimeOffsetVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }
    }
}
