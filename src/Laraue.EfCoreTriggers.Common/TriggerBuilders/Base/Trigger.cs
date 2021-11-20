using System.Collections.Generic;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Base
{
    public abstract class Trigger<TTriggerEntity> : ISqlConvertible
        where TTriggerEntity : class
    {
        public TriggerEvent TriggerEvent { get; }

        public TriggerTime TriggerTime { get; }

        public readonly List<ISqlConvertible> Actions = new ();

        protected Trigger(TriggerEvent triggerEvent, TriggerTime triggerTime)
        {
            TriggerTime = triggerTime;
            TriggerEvent = triggerEvent;
        }

        public virtual SqlBuilder BuildSql(ITriggerProvider visitor) => visitor.GetTriggerSql(this);

        public string Name => $"{Constants.AnnotationKey}_{TriggerTime}_{TriggerEvent}_{typeof(TTriggerEntity).Name}".ToUpper();
    }
}
