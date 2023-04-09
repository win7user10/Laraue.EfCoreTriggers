using System;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String
{
    /// <summary>
    /// Base visitor for <see cref="System.String"/> methods.
    /// </summary>
    public abstract class BaseStringVisitor : BaseMethodCallVisitor
    {
        /// <inheritdoc />
        protected override Type ReflectedType => typeof(string);

        /// <inheritdoc />
        protected BaseStringVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }
    }
}