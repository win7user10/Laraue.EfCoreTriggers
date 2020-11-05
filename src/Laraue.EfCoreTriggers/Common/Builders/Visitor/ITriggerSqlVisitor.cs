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

        string GetTriggerUpsertActionSql<TTriggerEntity, TUpdateEntity>(TriggerUpsertAction<TTriggerEntity, TUpdateEntity> triggerUpsertAction)
            where TTriggerEntity : class
            where TUpdateEntity : class;

        string GetTriggerDeleteActionSql<TTriggerEntity, TUpdateEntity>(TriggerDeleteAction<TTriggerEntity, TUpdateEntity> triggerDeleteAction)
            where TTriggerEntity : class
            where TUpdateEntity : class;
    }
}
