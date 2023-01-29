using System;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.OnUpdate
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
            Actions.AddRange(actionTrigger.ActionExpressions);
            Conditions.AddRange(actionTrigger.ActionConditions);
            return this;
        }
    }
}
