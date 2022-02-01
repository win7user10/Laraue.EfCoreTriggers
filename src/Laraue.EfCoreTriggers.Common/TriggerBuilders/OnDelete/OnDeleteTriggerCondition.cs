using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Laraue.EfCoreTriggers.Common.v2;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.OnDelete
{
    public class OnDeleteTriggerCondition<TTriggerEntity> : TriggerCondition
        where TTriggerEntity : class
    {
        public OnDeleteTriggerCondition(Expression<Func<TTriggerEntity, bool>> condition) 
            : base(condition)
        {
        }

        internal override ArgumentTypes ConditionPrefixes => new()
        {
            [Condition.Parameters[0].Name] = ArgumentType.Old,
        };
    }
}
