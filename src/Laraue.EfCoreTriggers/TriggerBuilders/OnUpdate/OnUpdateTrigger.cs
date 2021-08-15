using System;
using Laraue.EfCoreTriggers.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.TriggerBuilders.OnUpdate
{
    public class OnUpdateTrigger<TTriggerEntity> : Trigger<TTriggerEntity>
        where TTriggerEntity : class
    {
        public OnUpdateTrigger(TriggerTime triggerType) : base(TriggerEvent.Update, triggerType)
        {
        }

        public OnUpdateTrigger<TTriggerEntity> Action(Action<OnUpdateTriggerActions<TTriggerEntity>> actionSetup)
        {
            var actionTrigger = new OnUpdateTriggerActions<TTriggerEntity>();
            actionSetup.Invoke(actionTrigger);
            Actions.Add(actionTrigger);
            return this;
        }
    }
}
