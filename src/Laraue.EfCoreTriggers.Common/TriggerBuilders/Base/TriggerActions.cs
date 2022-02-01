using System.Collections.Generic;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Base
{
    public abstract class TriggerActions
    {
        public readonly List<ITriggerAction> ActionConditions = new ();

        public readonly List<ITriggerAction> ActionExpressions = new ();

        private void AddAction(ITriggerAction triggerAction)
            => ActionExpressions.Add(triggerAction);

        protected void Update(TriggerUpdateAction updateAction) => AddAction(updateAction);

        protected void Upsert(TriggerUpsertAction upsertAction) => AddAction(upsertAction);

        protected void Delete(TriggerDeleteAction deleteAction) => AddAction(deleteAction);

        protected void Insert(TriggerInsertAction insertAction) => AddAction(insertAction);

        protected void RawSql(TriggerRawAction rawSqlAction) => AddAction(rawSqlAction);
    }
}