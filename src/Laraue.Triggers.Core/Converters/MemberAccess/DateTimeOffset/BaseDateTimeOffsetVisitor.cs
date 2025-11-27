using System;
using Laraue.Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Triggers.Core.Converters.MemberAccess.DateTimeOffset
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
