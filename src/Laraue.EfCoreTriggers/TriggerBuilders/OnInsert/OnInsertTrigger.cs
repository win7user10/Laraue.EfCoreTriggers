using System;
using Laraue.EfCoreTriggers.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.TriggerBuilders.OnInsert
{
    public class OnInsertTrigger<TTriggerEntity> : Trigger<TTriggerEntity>
        where TTriggerEntity : class
    {
        public OnInsertTrigger(TriggerTime triggerType) : base(TriggerEvent.Insert, triggerType)
        {
        }

        public OnInsertTrigger<TTriggerEntity> Action(Action<OnInsertTriggerActions<TTriggerEntity>> actionSetup)
        {
            var actionTrigger = new OnInsertTriggerActions<TTriggerEntity>();
            actionSetup.Invoke(actionTrigger);
            Actions.Add(actionTrigger);
            return this;
        }
    }
}
