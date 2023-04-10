using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Abstractions;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Actions
{
    public sealed class TriggerUpdateAction : ITriggerAction
    {
        internal readonly LambdaExpression UpdateFilter;
        internal readonly LambdaExpression UpdateExpression;

        /// <summary>
        /// Initializes a new instance of <see cref="TriggerUpdateAction"/>.
        /// </summary>
        /// <param name="updateFilter"></param>
        /// <param name="updateExpression"></param>
        public TriggerUpdateAction(
            LambdaExpression updateFilter,
            LambdaExpression updateExpression)
        {
            UpdateFilter = updateFilter;
            UpdateExpression = updateExpression;
        }
    }
}