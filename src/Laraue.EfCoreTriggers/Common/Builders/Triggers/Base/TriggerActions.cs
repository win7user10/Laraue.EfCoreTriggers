using Laraue.EfCoreTriggers.Common.Builders.Visitor;
using System.Collections.Generic;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.Base
{
    public abstract class TriggerActions<TTriggerEntity> : ISqlConvertible
        where TTriggerEntity : class
    {
        public readonly List<ISqlConvertible> ActionConditions = new List<ISqlConvertible>();

        public readonly List<ISqlConvertible> ActionExpressions = new List<ISqlConvertible>();

        public virtual string BuildSql(ITriggerSqlVisitor visitor)
        {
            return visitor.GetTriggerActionsSql(this);
        }
    }
}