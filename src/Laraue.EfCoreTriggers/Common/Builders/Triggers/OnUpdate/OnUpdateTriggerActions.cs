using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using System;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.OnUpdate
{
    public class OnUpdateTriggerActions<TTriggerEntity> : TriggerActions<TTriggerEntity>
        where TTriggerEntity : class
    {
        public OnUpdateTriggerActions<TTriggerEntity> Condition(Expression<Func<TTriggerEntity, TTriggerEntity, bool>> condition)
        {
            ActionConditions.Add(new OnUpdateTriggerCondition<TTriggerEntity>(condition));
            return this;
        }

        public OnUpdateTriggerActions<TTriggerEntity> Update<TUpdateEntity>(
                Expression<Func<TTriggerEntity, TTriggerEntity, TUpdateEntity, bool>> entityFilter,
                Expression<Func<TTriggerEntity, TTriggerEntity, TUpdateEntity, TUpdateEntity>> setValues)
            where TUpdateEntity : class
        {
            Update(new OnUpdateTriggerUpdateAction<TTriggerEntity, TUpdateEntity>(entityFilter, setValues));
            return this;
        }

        public OnUpdateTriggerActions<TTriggerEntity> Upsert<TUpsertEntity>(
            Expression<Func<TUpsertEntity, object>> matchExpression,
            Expression<Func<TTriggerEntity, TTriggerEntity, TUpsertEntity>> insertExpression,
            Expression<Func<TTriggerEntity, TTriggerEntity, TUpsertEntity, TUpsertEntity>> onMatchExpression)
            where TUpsertEntity : class
        {
            Upsert(new OnUpdateTriggerUpsertAction<TTriggerEntity, TUpsertEntity>(matchExpression, insertExpression, onMatchExpression));
            return this;
        }

        public OnUpdateTriggerActions<TTriggerEntity> InsertIfNotExists<TUpsertEntity>(
            Expression<Func<TUpsertEntity, object>> matchExpression,
            Expression<Func<TTriggerEntity, TTriggerEntity, TUpsertEntity>> insertExpression)
            where TUpsertEntity : class
        {
            Upsert(new OnUpdateTriggerUpsertAction<TTriggerEntity, TUpsertEntity>(matchExpression, insertExpression, null));
            return this;
        }

        public OnUpdateTriggerActions<TTriggerEntity> Delete<TDeleteEntity>(Expression<Func<TTriggerEntity, TTriggerEntity, TDeleteEntity, bool>> deleteFilter)
            where TDeleteEntity : class
        {
            Delete(new OnUpdateTriggerDeleteAction<TTriggerEntity, TDeleteEntity>(deleteFilter));
            return this;
        }

        public OnUpdateTriggerActions<TTriggerEntity> Insert<TInsertEntity>(Expression<Func<TTriggerEntity, TTriggerEntity, TInsertEntity, TInsertEntity>> setValues)
            where TInsertEntity : class
        {
            Insert(new OnUpdateTriggerInsertAction<TTriggerEntity, TInsertEntity>(setValues));
            return this;
        }
    }
}