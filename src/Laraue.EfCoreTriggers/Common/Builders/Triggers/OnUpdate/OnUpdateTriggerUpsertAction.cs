using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Laraue.EfCoreTriggers.Common.Builders.Visitor;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.OnUpdate
{
    public class OnUpdateTriggerUpsertAction<TTriggerEntity, TUpsertEntity> : TriggerUpsertAction<TTriggerEntity, TUpsertEntity>
        where TTriggerEntity : class
        where TUpsertEntity : class
    {
        public OnUpdateTriggerUpsertAction(
            Expression<Func<TUpsertEntity, object>> matchExpression,
            Expression<Func<TTriggerEntity, TTriggerEntity, TUpsertEntity>> insertExpression,
            Expression<Func<TTriggerEntity, TTriggerEntity, TUpsertEntity, TUpsertEntity>> onMatchExpression)
                : base(matchExpression, insertExpression, onMatchExpression)
        {
        }

        public override Dictionary<string, ArgumentPrefix> InsertExpressionPrefixes => new Dictionary<string, ArgumentPrefix>
        {
            [InsertExpression.Parameters[0].Name] = ArgumentPrefix.Old,
            [InsertExpression.Parameters[1].Name] = ArgumentPrefix.New,
        };

        public override Dictionary<string, ArgumentPrefix> OnMatchExpressionPrefixes => new Dictionary<string, ArgumentPrefix>
        {
            [OnMatchExpression.Parameters[0].Name] = ArgumentPrefix.Old,
            [OnMatchExpression.Parameters[1].Name] = ArgumentPrefix.New,
        };
    }
}