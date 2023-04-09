using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Abstractions;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Actions
{
    /// <summary>
    /// Trigger condition.
    /// </summary>
    public class TriggerCondition : ITriggerAction
    {
        /// <summary>
        /// Expression to delete, e.g. Users.Where(x => x.Id == 2)
        /// </summary>
        internal readonly LambdaExpression Condition;
        
        /// <summary>
        /// Initializes a new instance of <see cref="TriggerCondition"/>.
        /// </summary>
        /// <param name="condition"></param>
        public TriggerCondition(LambdaExpression condition)
        {
            Condition = condition;
        }
    }
}