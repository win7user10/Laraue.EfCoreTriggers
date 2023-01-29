using System;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.OnDelete
{
    public class OnDeleteTrigger<TTriggerEntity> : Trigger<TTriggerEntity>
        where TTriggerEntity : class
    {
        public OnDeleteTrigger(TriggerTime triggerType) 
            : base(TriggerEvent.Delete, triggerType)
        {
        }

        public OnDeleteTrigger<TTriggerEntity> Action(Action<OnDeleteTriggerActions<TTriggerEntity>> actionSetup)
        {
            var actionTrigger = new OnDeleteTriggerActions<TTriggerEntity>();
            actionSetup.Invoke(actionTrigger);
            Actions.AddRange(actionTrigger.ActionExpressions);
            Conditions.AddRange(actionTrigger.ActionConditions);
            return this;
        }
    }
}
