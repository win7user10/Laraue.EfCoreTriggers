using Laraue.EfCoreTriggers.Common.Builders.Providers;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.Base
{
    public abstract class TriggerUpsertAction<TTriggerEntity, TUpsertEntity> : ITriggerAction
       where TTriggerEntity : class
       where TUpsertEntity : class
    {
        internal LambdaExpression MatchExpression;
        internal LambdaExpression InsertExpression;
        internal LambdaExpression OnMatchExpression;

        public TriggerUpsertAction(
            LambdaExpression matchExpression,
            LambdaExpression insertExpression,
            LambdaExpression onMatchExpression)
        {
            MatchExpression = matchExpression;
            InsertExpression = insertExpression;
            OnMatchExpression = onMatchExpression;
        }

        public virtual SqlBuilder BuildSql(ITriggerProvider visitor)
            => visitor.GetTriggerUpsertActionSql(this);

        internal abstract Dictionary<string, ArgumentType> InsertExpressionPrefixes { get; }

        internal abstract Dictionary<string, ArgumentType> OnMatchExpressionPrefixes { get; }

        internal abstract Dictionary<string, ArgumentType> MatchExpressionPrefixes { get; }
    }
}