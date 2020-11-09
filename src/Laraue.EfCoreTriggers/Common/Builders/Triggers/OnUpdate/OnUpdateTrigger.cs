using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using System;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.OnUpdate
{
    public class OnUpdateTrigger<TTriggerEntity> : Trigger<TTriggerEntity>
        where TTriggerEntity : class
    {
        public OnUpdateTrigger(TriggerTime triggerTime) : base(TriggerType.Update, triggerTime)
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
