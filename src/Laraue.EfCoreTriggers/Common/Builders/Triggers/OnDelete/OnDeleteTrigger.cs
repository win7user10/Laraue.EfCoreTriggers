using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using System;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.OnDelete
{
    public class OnDeleteTrigger<TTriggerEntity> : Trigger<TTriggerEntity>
        where TTriggerEntity : class
    {
        public OnDeleteTrigger(TriggerType triggerType) : base(TriggerAction.Delete, triggerType)
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
