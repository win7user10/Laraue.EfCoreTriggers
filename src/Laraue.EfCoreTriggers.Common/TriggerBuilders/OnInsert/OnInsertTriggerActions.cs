using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.OnInsert
{
    public class OnInsertTriggerActions<TTriggerEntity> : TriggerActions<TTriggerEntity>
        where TTriggerEntity : class
    {
        public OnInsertTriggerActions<TTriggerEntity> Condition(Expression<Func<TTriggerEntity, bool>> condition)
        {
            ActionConditions.Add(new OnInsertTriggerCondition<TTriggerEntity>(condition));
            return this;
        }

        public OnInsertTriggerActions<TTriggerEntity> Update<TUpdateEntity>(
            Expression<Func<TTriggerEntity, TUpdateEntity, bool>> entityFilter,
            Expression<Func<TTriggerEntity, TUpdateEntity, TUpdateEntity>> setValues)
            where TUpdateEntity : class
        {
            Update(new OnInsertTriggerUpdateAction<TTriggerEntity, TUpdateEntity>(entityFilter, setValues));
            return this;
        }

        public OnInsertTriggerActions<TTriggerEntity> Upsert<TUpsertEntity>(
            Expression<Func<TTriggerEntity, TUpsertEntity>> matchExpression,
            Expression<Func<TTriggerEntity, TUpsertEntity>> insertExpression,
            Expression<Func<TTriggerEntity, TUpsertEntity, TUpsertEntity>> onMatchExpression)
            where TUpsertEntity : class
        {
            Upsert(new OnInsertTriggerUpsertAction<TTriggerEntity, TUpsertEntity>(matchExpression, insertExpression, onMatchExpression));
            return this;
        }

        public OnInsertTriggerActions<TTriggerEntity> InsertIfNotExists<TUpsertEntity>(
            Expression<Func<TTriggerEntity, TUpsertEntity>> matchExpression,
            Expression<Func<TTriggerEntity, TUpsertEntity>> insertExpression)
            where TUpsertEntity : class
        {
            Upsert(new OnInsertTriggerUpsertAction<TTriggerEntity, TUpsertEntity>(matchExpression, insertExpression, null));
            return this;
        }

        public OnInsertTriggerActions<TTriggerEntity> Delete<TDeleteEntity>(Expression<Func<TTriggerEntity, TDeleteEntity, bool>> deleteFilter)
            where TDeleteEntity : class
        {
            Delete(new OnInsertTriggerDeleteAction<TTriggerEntity, TDeleteEntity>(deleteFilter));
            return this;
        }

        public OnInsertTriggerActions<TTriggerEntity> Insert<TInsertEntity>(Expression<Func<TTriggerEntity, TInsertEntity>> setValues)
            where TInsertEntity : class
        {
            Insert(new OnInsertTriggerInsertAction<TTriggerEntity, TInsertEntity>(setValues));
            return this;
        }
        
        public OnInsertTriggerActions<TTriggerEntity> ExecuteRawSql(string sql, params Expression<Func<TTriggerEntity, object>>[] argumentSelectors)
        {
            RawSql(new OnInsertTriggerRawSqlAction<TTriggerEntity>(sql, argumentSelectors));
            return this;
        }
    }
}
