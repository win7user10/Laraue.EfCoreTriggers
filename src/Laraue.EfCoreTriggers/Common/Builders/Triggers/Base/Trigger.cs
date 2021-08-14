using Laraue.EfCoreTriggers.Common.Builders.Providers;
using System.Collections.Generic;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.Base
{
    public abstract class Trigger<TTriggerEntity> : ISqlConvertible
        where TTriggerEntity : class
    {
        public TriggerEvent TriggerEvent { get; }

        public TriggerTime TriggerTime { get; }

        public readonly List<ISqlConvertible> Actions = new ();

        public Trigger(TriggerEvent triggerEvent, TriggerTime triggerTime)
        {
            TriggerTime = triggerTime;
            TriggerEvent = triggerEvent;
        }

        public virtual SqlBuilder BuildSql(ITriggerProvider visitor) => visitor.GetTriggerSql(this);

        public string Name => $"{Constants.AnnotationKey}_{TriggerTime}_{TriggerEvent}_{typeof(TTriggerEntity).Name}".ToUpper();
    }
}
