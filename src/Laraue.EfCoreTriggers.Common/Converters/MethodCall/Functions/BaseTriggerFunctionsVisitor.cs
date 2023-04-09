using System;
using Laraue.EfCoreTriggers.Common.Functions;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Functions
{
    /// <summary>
    /// Base visitor for the <see cref="TriggerFunctions"/>.
    /// </summary>
    public abstract class BaseTriggerFunctionsVisitor : BaseMethodCallVisitor
    {
        /// Initializes a new instance of <see cref="BaseTriggerFunctionsVisitor"/>.
        protected BaseTriggerFunctionsVisitor(IExpressionVisitorFactory visitorFactory)
            : base(visitorFactory)
        {
        }

        /// <inheritdoc />
        protected override Type ReflectedType => typeof(TriggerFunctions);
    }
}