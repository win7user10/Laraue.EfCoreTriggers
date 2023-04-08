using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Base
{
    public sealed class TriggerDeleteAction : ITriggerAction
    {
        /// <summary>
        /// Expression to delete, e.g. Users.Where(x => x.Id == 2)
        /// </summary>
        internal LambdaExpression DeletePredicate;

        public TriggerDeleteAction(LambdaExpression deletePredicate)
        {
            DeletePredicate = deletePredicate;
        }

        internal ArgumentTypes DeleteFilterPrefixes { get; }
    }
}