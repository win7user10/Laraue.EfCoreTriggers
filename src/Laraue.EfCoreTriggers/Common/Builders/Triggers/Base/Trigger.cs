using Laraue.EfCoreTriggers.Common.Builders.Visitor;
using System.Collections.Generic;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.Base
{
    public abstract class Trigger<TTriggerEntity> : ISqlConvertible
        where TTriggerEntity : class
    {
        internal TriggerType TriggerType { get; }

        internal TriggerTime TriggerTime { get; }

        internal readonly List<ISqlConvertible> Actions = new List<ISqlConvertible>();

        public Trigger(TriggerType triggerType, TriggerTime triggerTime)
        {
            TriggerType = triggerType;
            TriggerTime = triggerTime;
        }

        public string BuildSql(ITriggerSqlVisitor visitor) => visitor.GetTriggerSql(this);

        internal string Name => $"{Constants.AnnotationKey}_{TriggerTime.ToString().ToUpper()}_{TriggerType.ToString().ToUpper()}_{typeof(TTriggerEntity).Name.ToUpper()}";
    }
}
