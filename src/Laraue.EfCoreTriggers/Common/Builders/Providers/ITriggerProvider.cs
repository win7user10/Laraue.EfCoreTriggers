using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;

namespace Laraue.EfCoreTriggers.Common.Builders.Providers
{
    public interface ITriggerProvider
    {
        SqlBuilder GetDropTriggerSql(string triggerName);

        SqlBuilder GetTriggerSql<TTriggerEntity>(Trigger<TTriggerEntity> trigger)
            where TTriggerEntity : class;

        SqlBuilder GetTriggerConditionSql<TTriggerEntity>(TriggerCondition<TTriggerEntity> triggerCondition)
            where TTriggerEntity : class;

        SqlBuilder GetTriggerActionsSql<TTriggerEntity>(TriggerActions<TTriggerEntity> triggerActions)
            where TTriggerEntity : class;

        SqlBuilder GetTriggerUpdateActionSql<TTriggerEntity, TUpdateEntity>(TriggerUpdateAction<TTriggerEntity, TUpdateEntity> triggerUpdateAction)
            where TTriggerEntity : class
            where TUpdateEntity : class;

        SqlBuilder GetTriggerUpsertActionSql<TTriggerEntity, TUpsertEntity>(TriggerUpsertAction<TTriggerEntity, TUpsertEntity> triggerUpsertAction)
            where TTriggerEntity : class
            where TUpsertEntity : class;

        SqlBuilder GetTriggerDeleteActionSql<TTriggerEntity, TDeleteEntity>(TriggerDeleteAction<TTriggerEntity, TDeleteEntity> triggerDeleteAction)
            where TTriggerEntity : class
            where TDeleteEntity : class;

        SqlBuilder GetTriggerInsertActionSql<TTriggerEntity, TInsertEntity>(TriggerInsertAction<TTriggerEntity, TInsertEntity> triggerInsertAction)
            where TTriggerEntity : class
            where TInsertEntity : class;
    }
}
