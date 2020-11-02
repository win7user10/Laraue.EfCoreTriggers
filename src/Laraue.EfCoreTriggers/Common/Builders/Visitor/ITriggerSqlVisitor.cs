using Laraue.EfCoreTriggers.Common.Builders.Triggers.OnUpdate;
using Laraue.EfCoreTriggers.Common.Builders.Triggers.OnDelete;
using Laraue.EfCoreTriggers.Common.Builders.Triggers.OnInsert;
using System;

namespace Laraue.EfCoreTriggers.Common.Builders.Visitor
{
    public interface ITriggerSqlVisitor
    {
        string GetDropTriggerSql(string triggerName, Type entityType);

        #region OnUpdateTriggers

        string GetTriggerSql<TTriggerEntity>(OnUpdateTrigger<TTriggerEntity> trigger)
            where TTriggerEntity : class;

        string GetTriggerConditionSql<TTriggerEntity>(OnUpdateTriggerCondition<TTriggerEntity> triggerCondition)
            where TTriggerEntity : class;

        string GetTriggerActionsSql<TTriggerEntity>(OnUpdateTriggerActions<TTriggerEntity> triggerActions)
            where TTriggerEntity : class;

        string GetTriggerUpdateActionSql<TTriggerEntity, TUpdateEntity>(OnUpdateTriggerUpdateAction<TTriggerEntity, TUpdateEntity> triggerUpdateAction)
            where TTriggerEntity : class
            where TUpdateEntity : class;

        #endregion

        #region OnDeleteTriggers

        string GetTriggerSql<TTriggerEntity>(OnDeleteTrigger<TTriggerEntity> trigger)
            where TTriggerEntity : class;

        string GetTriggerConditionSql<TTriggerEntity>(OnDeleteTriggerCondition<TTriggerEntity> triggerCondition)
            where TTriggerEntity : class;

        string GetTriggerActionsSql<TTriggerEntity>(OnDeleteTriggerActions<TTriggerEntity> triggerActions)
            where TTriggerEntity : class;

        string GetTriggerUpdateActionSql<TTriggerEntity, TUpdateEntity>(OnDeleteTriggerUpdateAction<TTriggerEntity, TUpdateEntity> triggerUpdateAction)
            where TTriggerEntity : class
            where TUpdateEntity : class;

        #endregion

        #region OnInsertTriggers

        string GetTriggerSql<TTriggerEntity>(OnInsertTrigger<TTriggerEntity> trigger)
            where TTriggerEntity : class;

        string GetTriggerConditionSql<TTriggerEntity>(OnInsertTriggerCondition<TTriggerEntity> triggerCondition)
            where TTriggerEntity : class;

        string GetTriggerActionsSql<TTriggerEntity>(OnInsertTriggerActions<TTriggerEntity> triggerActions)
            where TTriggerEntity : class;

        string GetTriggerUpdateActionSql<TTriggerEntity, TUpdateEntity>(OnInsertTriggerUpdateAction<TTriggerEntity, TUpdateEntity> triggerUpdateAction)
            where TTriggerEntity : class
            where TUpdateEntity : class;

        #endregion
    }
}
