using System;
using Laraue.EfCoreTriggers.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.TriggerBuilders.OnDelete
{
    public class OnDeleteTrigger<TTriggerEntity> : Trigger<TTriggerEntity>
        where TTriggerEntity : class
    {
        public OnDeleteTrigger(TriggerTime triggerType) : base(TriggerEvent.Delete, triggerType)
        {
        }

        public OnDeleteTrigger<TTriggerEntity> Action(Action<OnDeleteTriggerActions<TTriggerEntity>> actionSetup)
        {
            var actionTrigger = new OnDeleteTriggerActions<TTriggerEntity>();
            actionSetup.Invoke(actionTrigger);
            Actions.Add(actionTrigger);
            return this;
        }
    }
}
