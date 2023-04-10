using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Abstractions;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Actions
{
    public sealed class TriggerDeleteAction : ITriggerAction
    {
        /// <summary>
        /// Expression to delete, e.g. Users.Where(x => x.Id == 2)
        /// </summary>
        internal readonly LambdaExpression DeletePredicate;

        /// <summary>
        /// Initializes a new instance of <see cref="TriggerDeleteAction"/>.
        /// </summary>
        /// <param name="deletePredicate"></param>
        public TriggerDeleteAction(LambdaExpression deletePredicate)
        {
            DeletePredicate = deletePredicate;
        }
    }
}