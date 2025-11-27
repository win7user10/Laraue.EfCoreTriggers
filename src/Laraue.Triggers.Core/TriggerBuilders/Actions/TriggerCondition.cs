using System.Linq.Expressions;
using Laraue.Triggers.Core.TriggerBuilders.Abstractions;

namespace Laraue.Triggers.Core.TriggerBuilders.Actions
{
    /// <summary>
    /// Trigger condition.
    /// </summary>
    public sealed class TriggerCondition : ITriggerAction
    {
        /// <summary>
        /// Predicate for an action, e.g. NEW.Age > 21
        /// </summary>
        public readonly LambdaExpression Predicate;
        
        /// <summary>
        /// Initializes a new instance of <see cref="TriggerCondition"/>.
        /// </summary>
        /// <param name="predicate"></param>
        public TriggerCondition(LambdaExpression predicate)
        {
            Predicate = predicate;
        }
    }
}