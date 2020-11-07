using System;
using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;

namespace Laraue.EfCoreTriggers.Common.Builders.Visitor
{
    public interface ITriggerSqlVisitor
    {
        string GetDropTriggerSql(string triggerName, Type entityType);

        string GetTriggerSql<TTriggerEntity>(Trigger<TTriggerEntity> trigger)
            where TTriggerEntity : class;

        string GetTriggerConditionSql<TTriggerEntity>(TriggerCondition<TTriggerEntity> triggerCondition)
            where TTriggerEntity : class;

        string GetTriggerActionsSql<TTriggerEntity>(TriggerActions<TTriggerEntity> triggerActions)
            where TTriggerEntity : class;

        string GetTriggerUpdateActionSql<TTriggerEntity, TUpdateEntity>(TriggerUpdateAction<TTriggerEntity, TUpdateEntity> triggerUpdateAction)
            where TTriggerEntity : class
            where TUpdateEntity : class;

        string GetTriggerUpsertActionSql<TTriggerEntity, TUpsertEntity>(TriggerUpsertAction<TTriggerEntity, TUpsertEntity> triggerUpsertAction)
            where TTriggerEntity : class
            where TUpsertEntity : class;

        string GetTriggerDeleteActionSql<TTriggerEntity, TDeleteEntity>(TriggerDeleteAction<TTriggerEntity, TDeleteEntity> triggerDeleteAction)
            where TTriggerEntity : class
            where TDeleteEntity : class;

        string GetTriggerInsertActionSql<TTriggerEntity, TInsertEntity>(TriggerInsertAction<TTriggerEntity, TInsertEntity> triggerInsertAction)
            where TTriggerEntity : class
            where TInsertEntity : class;
    }
}
