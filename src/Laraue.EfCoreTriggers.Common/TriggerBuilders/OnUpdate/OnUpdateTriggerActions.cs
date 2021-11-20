using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.OnUpdate
{
    public class OnUpdateTriggerActions<TTriggerEntity> : TriggerActions<TTriggerEntity>
        where TTriggerEntity : class
    {
        /// <summary>
        /// Add the condition to the after update trigger action. Action will be executed only when the condition passed.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public OnUpdateTriggerActions<TTriggerEntity> Condition(Expression<Func<TTriggerEntity, TTriggerEntity, bool>> condition)
        {
            ActionConditions.Add(new OnUpdateTriggerCondition<TTriggerEntity>(condition));
            return this;
        }

        /// <summary>
        /// Execute update query in a database when a record was updated.
        /// </summary>
        /// <param name="entityFilter"></param>
        /// <param name="setValues"></param>
        /// <typeparam name="TUpdateEntity"></typeparam>
        /// <returns></returns>
        public OnUpdateTriggerActions<TTriggerEntity> Update<TUpdateEntity>(
                Expression<Func<TTriggerEntity, TTriggerEntity, TUpdateEntity, bool>> entityFilter,
                Expression<Func<TTriggerEntity, TTriggerEntity, TUpdateEntity, TUpdateEntity>> setValues)
            where TUpdateEntity : class
        {
            Update(new OnUpdateTriggerUpdateAction<TTriggerEntity, TUpdateEntity>(entityFilter, setValues));
            return this;
        }

        /// <summary>
        /// Execute upsert in a database when a record was updated.
        /// </summary>
        /// <param name="matchExpression"></param>
        /// <param name="insertExpression"></param>
        /// <param name="onMatchExpression"></param>
        /// <typeparam name="TUpsertEntity"></typeparam>
        /// <returns></returns>
        public OnUpdateTriggerActions<TTriggerEntity> Upsert<TUpsertEntity>(
            Expression<Func<TTriggerEntity, TUpsertEntity, TUpsertEntity>> matchExpression,
            Expression<Func<TTriggerEntity, TTriggerEntity, TUpsertEntity>> insertExpression,
            Expression<Func<TTriggerEntity, TTriggerEntity, TUpsertEntity, TUpsertEntity>> onMatchExpression)
            where TUpsertEntity : class
        {
            Upsert(new OnUpdateTriggerUpsertAction<TTriggerEntity, TUpsertEntity>(matchExpression, insertExpression, onMatchExpression));
            return this;
        }

        /// <summary>
        /// Execute insert not existing entity in a database when a record was updated.
        /// </summary>
        /// <param name="matchExpression"></param>
        /// <param name="insertExpression"></param>
        /// <typeparam name="TUpsertEntity"></typeparam>
        /// <returns></returns>
        public OnUpdateTriggerActions<TTriggerEntity> InsertIfNotExists<TUpsertEntity>(
            Expression<Func<TTriggerEntity, TUpsertEntity, TUpsertEntity>> matchExpression,
            Expression<Func<TTriggerEntity, TTriggerEntity, TUpsertEntity>> insertExpression)
            where TUpsertEntity : class
        {
            Upsert(new OnUpdateTriggerUpsertAction<TTriggerEntity, TUpsertEntity>(matchExpression, insertExpression, null));
            return this;
        }

        /// <summary>
        /// Execute delete from a database when a record was updated.
        /// </summary>
        /// <param name="deleteFilter"></param>
        /// <typeparam name="TDeleteEntity"></typeparam>
        /// <returns></returns>
        public OnUpdateTriggerActions<TTriggerEntity> Delete<TDeleteEntity>(Expression<Func<TTriggerEntity, TTriggerEntity, TDeleteEntity, bool>> deleteFilter)
            where TDeleteEntity : class
        {
            Delete(new OnUpdateTriggerDeleteAction<TTriggerEntity, TDeleteEntity>(deleteFilter));
            return this;
        }

        /// <summary>
        /// Execute insert in a database when a record was updated.
        /// </summary>
        /// <param name="setValues"></param>
        /// <typeparam name="TInsertEntity"></typeparam>
        /// <returns></returns>
        public OnUpdateTriggerActions<TTriggerEntity> Insert<TInsertEntity>(Expression<Func<TTriggerEntity, TTriggerEntity, TInsertEntity>> setValues)
            where TInsertEntity : class
        {
            Insert(new OnUpdateTriggerInsertAction<TTriggerEntity, TInsertEntity>(setValues));
            return this;
        }
        
        /// <summary>
        /// Execute raw sql query in a database when a record was updated.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="argumentSelectors"></param>
        /// <returns></returns>
        public OnUpdateTriggerActions<TTriggerEntity> ExecuteRawSql(string sql,
            params Expression<Func<TTriggerEntity, TTriggerEntity, object>>[] argumentSelectors)
        {
            RawSql(new OnUpdateTriggerRawSqlAction<TTriggerEntity>(sql, argumentSelectors));
            return this;
        }
    }
}