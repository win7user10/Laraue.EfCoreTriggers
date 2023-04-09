using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Base
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