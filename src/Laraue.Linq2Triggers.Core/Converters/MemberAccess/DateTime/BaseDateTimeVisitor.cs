using System;
using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Linq2Triggers.Core.Converters.MemberAccess.DateTime
{
    /// <summary>
    /// Base visitor for <see cref="System.Math"/> methods.
    /// </summary>
    public abstract class BaseDateTimeVisitor : BaseMemberAccessVisitor
    {
        /// <inheritdoc />
        protected override Type ReflectedType => typeof(System.DateTime);

        /// <inheritdoc />
        protected BaseDateTimeVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }
    }
}
