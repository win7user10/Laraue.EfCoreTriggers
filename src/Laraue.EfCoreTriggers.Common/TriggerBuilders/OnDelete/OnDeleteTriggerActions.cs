using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.OnDelete
{
    public class OnDeleteTriggerActions<TTriggerEntity> : TriggerActions<TTriggerEntity>
        where TTriggerEntity : class
    {
        /// <summary>
        /// Add the condition to the delete trigger action. Action will be executed only when the condition passed.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public OnDeleteTriggerActions<TTriggerEntity> Condition(Expression<Func<TTriggerEntity, bool>> condition)
        {
            ActionConditions.Add(new OnDeleteTriggerCondition<TTriggerEntity>(condition));
            return this;
        }

        /// <summary>
        /// Execute update query in a database when a record was deleted.
        /// </summary>
        /// <param name="entityFilter"></param>
        /// <param name="setValues"></param>
        /// <typeparam name="TUpdateEntity"></typeparam>
        /// <returns></returns>
        public OnDeleteTriggerActions<TTriggerEntity> Update<TUpdateEntity>(
                Expression<Func<TTriggerEntity, TUpdateEntity, bool>> entityFilter,
                Expression<Func<TTriggerEntity, TUpdateEntity, TUpdateEntity>> setValues)
            where TUpdateEntity : class
        {
            Update(new OnDeleteTriggerUpdateAction<TTriggerEntity, TUpdateEntity>(entityFilter, setValues));
            return this;
        }

        /// <summary>
        /// Execute upsert in a database when a record was deleted.
        /// </summary>
        /// <param name="matchExpression"></param>
        /// <param name="insertExpression"></param>
        /// <param name="onMatchExpression"></param>
        /// <typeparam name="TUpsertEntity"></typeparam>
        /// <returns></returns>
        public OnDeleteTriggerActions<TTriggerEntity> Upsert<TUpsertEntity>(
            Expression<Func<TTriggerEntity, TUpsertEntity>> matchExpression,
            Expression<Func<TTriggerEntity, TUpsertEntity>> insertExpression,
            Expression<Func<TTriggerEntity, TUpsertEntity, TUpsertEntity>> onMatchExpression)
            where TUpsertEntity : class
        {
            Upsert(new OnDeleteTriggerUpsertAction<TTriggerEntity, TUpsertEntity>(matchExpression, insertExpression, onMatchExpression));
            return this;
        }

        /// <summary>
        /// Execute insert not existing entity in a database when a record was deleted.
        /// </summary>
        /// <param name="matchExpression"></param>
        /// <param name="insertExpression"></param>
        /// <typeparam name="TUpsertEntity"></typeparam>
        /// <returns></returns>
        public OnDeleteTriggerActions<TTriggerEntity> InsertIfNotExists<TUpsertEntity>(
            Expression<Func<TTriggerEntity, TUpsertEntity>> matchExpression,
            Expression<Func<TTriggerEntity, TUpsertEntity>> insertExpression)
            where TUpsertEntity : class
        {
            Upsert(new OnDeleteTriggerUpsertAction<TTriggerEntity, TUpsertEntity>(matchExpression, insertExpression, null));
            return this;
        }

        /// <summary>
        /// Execute delete from a database when a record was deleted.
        /// </summary>
        /// <param name="deleteExpression"></param>
        /// <typeparam name="TDeleteEntity"></typeparam>
        /// <returns></returns>
        public OnDeleteTriggerActions<TTriggerEntity> Delete<TDeleteEntity>(Expression<Func<TTriggerEntity, TDeleteEntity, bool>> deleteExpression)
            where TDeleteEntity : class
        {
            Delete(new OnDeleteTriggerDeleteAction<TTriggerEntity, TDeleteEntity>(deleteExpression));
            return this;
        }

        /// <summary>
        /// Execute insert in a database when a record was deleted.
        /// </summary>
        /// <param name="setValues"></param>
        /// <typeparam name="TInsertEntity"></typeparam>
        /// <returns></returns>
        public OnDeleteTriggerActions<TTriggerEntity> Insert<TInsertEntity>(Expression<Func<TTriggerEntity, TInsertEntity>> setValues)
            where TInsertEntity : class
        {
            Insert(new OnDeleteTriggerInsertAction<TTriggerEntity, TInsertEntity>(setValues));
            return this;
        }
        
        /// <summary>
        /// Execute raw sql query in a database when a record was deleted.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="argumentSelectors"></param>
        /// <returns></returns>
        public OnDeleteTriggerActions<TTriggerEntity> ExecuteRawSql(string sql, params Expression<Func<TTriggerEntity, object>>[] argumentSelectors)
        {
            RawSql(new OnDeleteTriggerRawSqlAction<TTriggerEntity>(sql, argumentSelectors));
            return this;
        }
    }
}