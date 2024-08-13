using System;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Guid
{
    /// <summary>
    /// Base visitor for <see cref="System.Math"/> methods.
    /// </summary>
    public abstract class BaseGuidVisitor : BaseMethodCallVisitor
    {
        /// <inheritdoc />
        protected override Type ReflectedType => typeof(System.Guid);

        /// <inheritdoc />
        protected BaseGuidVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }
    }
}
