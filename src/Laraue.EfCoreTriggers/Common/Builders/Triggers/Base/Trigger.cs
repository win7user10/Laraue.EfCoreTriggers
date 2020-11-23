using Laraue.EfCoreTriggers.Common.Builders.Providers;
using System.Collections.Generic;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.Base
{
    public abstract class Trigger<TTriggerEntity> : ISqlConvertible
        where TTriggerEntity : class
    {
        internal TriggerEvent TriggerEvent { get; }

        internal TriggerTime TriggerTime { get; }

        internal readonly List<ISqlConvertible> Actions = new List<ISqlConvertible>();

        public Trigger(TriggerEvent triggerEvent, TriggerTime triggerTime)
        {
            TriggerTime = triggerTime;
            TriggerEvent = triggerEvent;
        }

        public virtual SqlBuilder BuildSql(ITriggerProvider visitor) => visitor.GetTriggerSql(this);

        internal string Name => $"{Constants.AnnotationKey}_{TriggerTime.ToString().ToUpper()}_{TriggerEvent.ToString().ToUpper()}_{typeof(TTriggerEntity).Name.ToUpper()}";
    }
}
