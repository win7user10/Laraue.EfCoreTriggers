using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Base
{
    public abstract class TriggerDeleteAction : ITriggerAction
    {
        /// <summary>
        /// Expression to delete, e.g. Users.Where(x => x.Id == 2)
        /// </summary>
        internal LambdaExpression DeletePredicate;

        protected TriggerDeleteAction(LambdaExpression deletePredicate)
        {
            DeletePredicate = deletePredicate;
        }

        internal abstract ArgumentTypes DeleteFilterPrefixes { get; }
    }
}