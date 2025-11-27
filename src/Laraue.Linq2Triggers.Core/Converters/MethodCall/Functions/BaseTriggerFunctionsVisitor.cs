using System;
using Laraue.Linq2Triggers.Core.Functions;
using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Linq2Triggers.Core.Converters.MethodCall.Functions
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