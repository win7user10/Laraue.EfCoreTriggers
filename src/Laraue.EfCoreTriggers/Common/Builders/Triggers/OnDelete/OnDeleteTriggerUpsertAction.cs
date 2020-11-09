using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Laraue.EfCoreTriggers.Common.Builders.Visitor;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.OnDelete
{
    public class OnDeleteTriggerUpsertAction<TTriggerEntity, TUpsertEntity> : TriggerUpsertAction<TTriggerEntity, TUpsertEntity>
        where TTriggerEntity : class
        where TUpsertEntity : class
    {
        public OnDeleteTriggerUpsertAction(
            Expression<Func<TUpsertEntity, object>> matchExpression,
            Expression<Func<TTriggerEntity, TUpsertEntity>> insertExpression,
            Expression<Func<TTriggerEntity, TUpsertEntity, TUpsertEntity>> onMatchExpression)
                : base(matchExpression, insertExpression, onMatchExpression)
        {
        }

        internal override Dictionary<string, ArgumentPrefix> InsertExpressionPrefixes => new Dictionary<string, ArgumentPrefix>
        {
            [InsertExpression.Parameters[0].Name] = ArgumentPrefix.Old,
        };

        internal override Dictionary<string, ArgumentPrefix> OnMatchExpressionPrefixes => OnMatchExpression is null
            ? new Dictionary<string, ArgumentPrefix>()
            : new Dictionary<string, ArgumentPrefix>
            {
                [OnMatchExpression.Parameters[0].Name] = ArgumentPrefix.Old,
            };
    }
}