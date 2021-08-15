using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.TriggerBuilders.OnDelete
{
    public class OnDeleteTriggerCondition<TTriggerEntity> : TriggerCondition<TTriggerEntity>
        where TTriggerEntity : class
    {
        public OnDeleteTriggerCondition(Expression<Func<TTriggerEntity, bool>> condition) : base(condition)
        {
        }

        internal override Dictionary<string, ArgumentType> ConditionPrefixes => new()
        {
            [Condition.Parameters[0].Name] = ArgumentType.Old,
        };
    }
}
