using Laraue.EfCoreTriggers.Common.Builders.Providers;
using System.Collections.Generic;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.Base
{
    public abstract class Trigger<TTriggerEntity> : ISqlConvertible
        where TTriggerEntity : class
    {
        internal TriggerAction TriggerAction { get; }

        internal TriggerType TriggerType { get; }

        internal readonly List<ISqlConvertible> Actions = new List<ISqlConvertible>();

        public Trigger(TriggerAction triggerAction, TriggerType triggerType)
        {
            TriggerType = triggerType;
            TriggerAction = triggerAction;
        }

        public virtual GeneratedSql BuildSql(ITriggerProvider visitor) => visitor.GetTriggerSql(this);

        internal string Name => $"{Constants.AnnotationKey}_{TriggerType.ToString().ToUpper()}_{TriggerAction.ToString().ToUpper()}_{typeof(TTriggerEntity).Name.ToUpper()}";
    }
}
