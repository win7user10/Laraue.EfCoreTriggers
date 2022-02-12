using System;
using System.Collections.Generic;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Base
{
    public abstract class Trigger<TTriggerEntity> : ITrigger
        where TTriggerEntity : class
    {
        public TriggerEvent TriggerEvent { get; }

        public TriggerTime TriggerTime { get; }

        public Type TriggerEntityType => typeof(TTriggerEntity);

        public List<ITriggerAction> Actions { get; } = new();

        public List<ITriggerAction> Conditions  { get; } = new();

        protected Trigger(TriggerEvent triggerEvent, TriggerTime triggerTime)
        {
            TriggerTime = triggerTime;
            TriggerEvent = triggerEvent;
        }

        public virtual string Name => $"{Constants.AnnotationKey}_{TriggerTime}_{TriggerEvent}_{typeof(TTriggerEntity).Name}".ToUpper();
    }
}
