using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.OnInsert
{
    public class OnInsertTriggerUpsertAction<TTriggerEntity, TUpsertEntity> : TriggerUpsertAction
        where TTriggerEntity : class
        where TUpsertEntity : class
    {
        public OnInsertTriggerUpsertAction(
            Expression<Func<TTriggerEntity, TUpsertEntity>> matchExpression,
            Expression<Func<TTriggerEntity, TUpsertEntity>> insertExpression,
            Expression<Func<TTriggerEntity, TUpsertEntity, TUpsertEntity>> onMatchExpression)
                : base(matchExpression, insertExpression, onMatchExpression)
        {
        }

        public override ArgumentTypes InsertExpressionPrefixes => new()
        {
            [InsertExpression.Parameters[0].Name] = ArgumentType.New,
        };

        public override ArgumentTypes OnMatchExpressionPrefixes => OnMatchExpression is null
            ? new ArgumentTypes()
            : new ArgumentTypes
            {
                [OnMatchExpression.Parameters[0].Name] = ArgumentType.New,
            };

        public override ArgumentTypes MatchExpressionPrefixes => new()
        {
            [MatchExpression.Parameters[0].Name] = ArgumentType.New,
        };
    }
}