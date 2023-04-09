using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Abstractions;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Actions
{
    public sealed class TriggerUpsertAction : ITriggerAction
    {
        public LambdaExpression MatchExpression;
        public LambdaExpression InsertExpression;
        public LambdaExpression? OnMatchExpression;

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