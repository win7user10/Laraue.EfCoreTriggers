using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Abstractions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Actions
{
    /// <summary>
    /// The group of actions. Transforms to SQL where all actions will be performed
    /// when all conditions will be passed.
    /// </summary>
    public abstract class TriggerActionsGroup : ITriggerAction
    {
        /// <summary>
        /// The list if all conditions in the actions group.
        /// </summary>
        public IEnumerable<ITriggerAction> ActionConditions => _actionConditions;
    
        /// <summary>
        /// The list if all actions in the actions group.
        /// </summary>
        public IEnumerable<ITriggerAction> ActionExpressions => _actionExpressions;

        private readonly List<TriggerCondition> _actionConditions = new();
        
        private readonly List<ITriggerAction> _actionExpressions = new();
    
        /// <summary>
        /// Adds new condition to the actions groups.
        /// </summary>
        /// <param name="triggerAction"></param>
        protected void AddAction(ITriggerAction triggerAction)
        {
            _actionExpressions.Add(triggerAction);
        }
    
        /// <summary>
        /// Adds new action to the actions groups.
        /// </summary>
        /// <param name="triggerAction"></param>
        protected void AddCondition(TriggerCondition triggerAction)
        {
            _actionConditions.Add(triggerAction);
        }
    }

    /// <inheritdoc />
    /// <typeparam name="TTriggerEntity">Type of the triggered entity.</typeparam>
    /// <typeparam name="TTriggerEntityRefs">Type of table references available in the current trigger.</typeparam>
    public sealed class TriggerActionsGroup<TTriggerEntity, TTriggerEntityRefs> : TriggerActionsGroup
        where TTriggerEntity : class
        where TTriggerEntityRefs : ITableRef<TTriggerEntity>
    {
        /// <summary>
        /// Adds the new condition to the action.
        /// The action will be fired only if all conditions passed.
        /// </summary>
        /// <param name="conditionalExpression"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public TriggerActionsGroup<TTriggerEntity, TTriggerEntityRefs> Condition(
            Expression<Func<TTriggerEntityRefs, bool>> conditionalExpression)
        {
            // Throw on expressions like "_ => true"
            if (conditionalExpression.Body is ConstantExpression)
            {
                throw new InvalidOperationException("Condition with constant expression makes no sense");
            }

            AddCondition(new TriggerCondition(conditionalExpression));
        
            return this;
        }

        /// <summary>
        /// Performs inserting of the new entity in the target table.
        /// </summary>
        /// <param name="insertExpression">Describes how to insert a new entity based on the <see cref="ITableRef"/></param>
        /// <typeparam name="TInsertEntity"></typeparam>
        /// <returns></returns>
        public TriggerActionsGroup<TTriggerEntity, TTriggerEntityRefs> Insert<TInsertEntity>(
            Expression<Func<TTriggerEntityRefs, TInsertEntity>> insertExpression)
        {
            AddAction(new TriggerInsertAction(insertExpression));

            return this;
        }
    
        /// <summary>
        /// Performs deleting of the entity from the target table. 
        /// </summary>
        /// <param name="deletePredicate"></param>
        /// <typeparam name="TDeleteEntity"></typeparam>
        /// <returns></returns>
        public TriggerActionsGroup<TTriggerEntity, TTriggerEntityRefs> Delete<TDeleteEntity>(
            Expression<Func<TTriggerEntityRefs, TDeleteEntity, bool>> deletePredicate)
        {
            AddAction(new TriggerDeleteAction(deletePredicate));

            return this;
        }
    
        public TriggerActionsGroup<TTriggerEntity, TTriggerEntityRefs> Update<TUpdateEntity>(
            Expression<Func<TTriggerEntityRefs, TUpdateEntity, bool>> updatePredicate,
            Expression<Func<TTriggerEntityRefs, TUpdateEntity, TUpdateEntity>> updateExpression)
        {
            AddAction(new TriggerUpdateAction(updatePredicate, updateExpression));

            return this;
        }
    
        public TriggerActionsGroup<TTriggerEntity, TTriggerEntityRefs> Upsert<TUpsertEntity>(
            Expression<Func<TTriggerEntityRefs, TUpsertEntity, bool>> upsertPredicate,
            Expression<Func<TTriggerEntityRefs, TUpsertEntity>> insertExpression,
            Expression<Func<TTriggerEntityRefs, TUpsertEntity, TUpsertEntity>> updateExpression)
        {
            AddAction(
                new TriggerUpsertAction(
                    upsertPredicate,
                    insertExpression,
                    updateExpression));

            return this;
        }
    
        public TriggerActionsGroup<TTriggerEntity, TTriggerEntityRefs> InsertIfNotExists<TInsertEntity>(
            Expression<Func<TTriggerEntityRefs, TInsertEntity, bool>> insertPredicate,
            Expression<Func<TTriggerEntityRefs, TInsertEntity>> insertExpression)
        {
            AddAction(
                new TriggerUpsertAction(
                    insertPredicate,
                    insertExpression,
                    null));

            return this;
        }
    
        public TriggerActionsGroup<TTriggerEntity, TTriggerEntityRefs> ExecuteRawSql(
            string sql,
            params Expression<Func<TTriggerEntityRefs, object>>[] getSqlVariable)
        {
            AddAction(new TriggerRawAction(sql, getSqlVariable.Cast<LambdaExpression>().ToArray()));

            return this;
        }
    }
}