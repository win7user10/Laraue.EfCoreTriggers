using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Abstractions;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Actions
{
    public sealed class TriggerUpsertAction : ITriggerAction
    {
        public readonly LambdaExpression MatchExpression;
        public readonly LambdaExpression InsertExpression;
        public readonly LambdaExpression? OnMatchExpression;

        /// <summary>
        /// Initializes a new instance of <see cref="TriggerUpsertAction"/>.
        /// </summary>
        /// <param name="matchExpression"></param>
        /// <param name="insertExpression"></param>
        /// <param name="onMatchExpression"></param>
        public TriggerUpsertAction(
            LambdaExpression matchExpression,
            LambdaExpression insertExpression,
            LambdaExpression? onMatchExpression)
        {
            MatchExpression = matchExpression;
            InsertExpression = insertExpression;
            OnMatchExpression = onMatchExpression;
        }
    }
}