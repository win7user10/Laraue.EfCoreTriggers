using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using System;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.OnDelete
{
    public class OnDeleteTriggerActions<TTriggerEntity> : TriggerActions<TTriggerEntity>
        where TTriggerEntity : class
    {
        public OnDeleteTriggerActions<TTriggerEntity> Condition(Expression<Func<TTriggerEntity, bool>> condition)
        {
            ActionConditions.Add(new OnDeleteTriggerCondition<TTriggerEntity>(condition));
            return this;
        }

        public OnDeleteTriggerActions<TTriggerEntity> Update<TUpdateEntity>(
                Expression<Func<TTriggerEntity, TUpdateEntity, bool>> entityFilter,
                Expression<Func<TTriggerEntity, TUpdateEntity, TUpdateEntity>> setValues)
            where TUpdateEntity : class
        {
            ActionExpressions.Add(new OnDeleteTriggerUpdateAction<TTriggerEntity, TUpdateEntity>(entityFilter, setValues));
            return this;
        }

        public OnDeleteTriggerActions<TTriggerEntity> Upsert<TUpsertEntity>(
            Expression<Func<TUpsertEntity, object>> matchExpression,
            Expression<Func<TTriggerEntity, TUpsertEntity>> insertExpression,
            Expression<Func<TTriggerEntity, TUpsertEntity, TUpsertEntity>> onMatchExpression)
            where TUpsertEntity : class
        {
            ActionExpressions.Add(new OnDeleteTriggerUpsertAction<TTriggerEntity, TUpsertEntity>(matchExpression, insertExpression, onMatchExpression));
            return this;
        }
    }
}
