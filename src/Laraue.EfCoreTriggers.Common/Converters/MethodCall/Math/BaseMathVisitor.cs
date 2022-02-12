using System;
using Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math
{
    /// <summary>
    /// Base visitor for <see cref="System.Math"/> methods.
    /// </summary>
    public abstract class BaseMathVisitor : BaseMethodCallVisitor
    {
        /// <inheritdoc />
        protected override Type ReflectedType => typeof(System.Math);

        /// <inheritdoc />
        protected BaseMathVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }
    }
}
