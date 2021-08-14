using Laraue.EfCoreTriggers.Common.Builders.Providers;
using System.Collections.Generic;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.Base
{
    public abstract class TriggerActions<TTriggerEntity> : ISqlConvertible
        where TTriggerEntity : class
    {
        public readonly List<ISqlConvertible> ActionConditions = new ();

        public readonly List<ITriggerAction> ActionExpressions = new ();

        public virtual SqlBuilder BuildSql(ITriggerProvider visitor)
            => visitor.GetTriggerActionsSql(this);

        internal void AddAction(ITriggerAction triggerAction)
            => ActionExpressions.Add(triggerAction);

        protected void Update<TUpdateEntity>(TriggerUpdateAction<TTriggerEntity, TUpdateEntity> updateAction)
            where TUpdateEntity : class => AddAction(updateAction);

        protected void Upsert<TUpsertEntity>(TriggerUpsertAction<TTriggerEntity, TUpsertEntity> upsertAction)
            where TUpsertEntity : class => AddAction(upsertAction);

        protected void Delete<TDeleteEntity>(TriggerDeleteAction<TTriggerEntity, TDeleteEntity> deleteAction)
            where TDeleteEntity : class => AddAction(deleteAction);

        protected void Insert<TInsertEntity>(TriggerInsertAction<TTriggerEntity, TInsertEntity> insertAction)
            where TInsertEntity : class => AddAction(insertAction);
    }
}