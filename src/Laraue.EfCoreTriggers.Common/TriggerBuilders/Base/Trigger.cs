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

        public IList<ITriggerAction> Actions { get; } = new List<ITriggerAction>();

        public IList<ITriggerAction> Conditions  { get; } = new List<ITriggerAction>();

        protected Trigger(TriggerEvent triggerEvent, TriggerTime triggerTime)
        {
            TriggerTime = triggerTime;
            TriggerEvent = triggerEvent;
        }

        public virtual string Name => $"{Constants.AnnotationKey}_{TriggerTime}_{TriggerEvent}_{typeof(TTriggerEntity).Name}".ToUpper();
    }
}
