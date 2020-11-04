using Laraue.EfCoreTriggers.Common.Builders.Visitor;
using System.Collections.Generic;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.Base
{
    public abstract class Trigger<TTriggerEntity> : ISqlConvertible
    {
        public TriggerType TriggerType { get; }

        public TriggerTime TriggerTime { get; }

        public readonly List<ISqlConvertible> Actions = new List<ISqlConvertible>();

        public Trigger(TriggerType triggerType, TriggerTime triggerTime)
        {
            TriggerType = triggerType;
            TriggerTime = triggerTime;
        }

        public abstract string BuildSql(ITriggerSqlVisitor visitor);

        public string Name => $"{Constants.AnnotationKey}_{TriggerTime.ToString().ToUpper()}_{TriggerType.ToString().ToUpper()}_{typeof(TTriggerEntity).Name.ToUpper()}";
    }
}
