using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

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
