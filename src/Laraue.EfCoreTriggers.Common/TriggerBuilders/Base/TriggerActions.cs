using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Base
{
    public abstract class TriggerActions
    {
        internal IEnumerable<ITriggerAction> ActionConditions => _actionConditions;

        internal IEnumerable<ITriggerAction> ActionExpressions => _actionExpressions;

        private readonly List<TriggerCondition> _actionConditions = new();
        
        private readonly List<ITriggerAction> _actionExpressions = new();
        
        private void AddAction(ITriggerAction triggerAction)
            => _actionExpressions.Add(triggerAction);

        internal void AddCondition(TriggerCondition triggerCondition)
        {
            // Throw on expressions like "_ => true"
            if (triggerCondition.Condition.Body is ConstantExpression)
            {
                throw new InvalidOperationException("Condition with constant expression makes no sense");
            }
            
            _actionConditions.Add(triggerCondition);
        }

        protected void Update(TriggerUpdateAction updateAction) => AddAction(updateAction);

        protected void Upsert(TriggerUpsertAction upsertAction) => AddAction(upsertAction);

        protected void Delete(TriggerDeleteAction deleteAction) => AddAction(deleteAction);

        protected void Insert(TriggerInsertAction insertAction) => AddAction(insertAction);

        protected void RawSql(TriggerRawAction rawSqlAction) => AddAction(rawSqlAction);
    }
}