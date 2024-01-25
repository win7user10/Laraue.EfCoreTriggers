using System;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MemberAccess.DateTime
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
