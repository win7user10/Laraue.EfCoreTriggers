using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.OnUpdate
{
    public class OnUpdateTriggerUpsertAction<TTriggerEntity, TUpsertEntity> : TriggerUpsertAction<TTriggerEntity, TUpsertEntity>
        where TTriggerEntity : class
        where TUpsertEntity : class
    {
        public OnUpdateTriggerUpsertAction(
            Expression<Func<TTriggerEntity, TUpsertEntity, TUpsertEntity>> matchExpression,
            Expression<Func<TTriggerEntity, TTriggerEntity, TUpsertEntity>> insertExpression,
            Expression<Func<TTriggerEntity, TTriggerEntity, TUpsertEntity, TUpsertEntity>> onMatchExpression)
                : base(matchExpression, insertExpression, onMatchExpression)
        {
        }

        public override Dictionary<string, ArgumentType> InsertExpressionPrefixes => new()
        {
            [InsertExpression.Parameters[0].Name] = ArgumentType.Old,
            [InsertExpression.Parameters[1].Name] = ArgumentType.New,
        };

        public override Dictionary<string, ArgumentType> OnMatchExpressionPrefixes => OnMatchExpression is null
            ? new Dictionary<string, ArgumentType>()
            : new Dictionary<string, ArgumentType>
            {
                [OnMatchExpression.Parameters[0].Name] = ArgumentType.Old,
                [OnMatchExpression.Parameters[1].Name] = ArgumentType.New,
            };

        public override Dictionary<string, ArgumentType> MatchExpressionPrefixes => new()
        {
            [MatchExpression.Parameters[0].Name] = ArgumentType.Old,
            [MatchExpression.Parameters[1].Name] = ArgumentType.New,
        };
    }
}