using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Laraue.EfCoreTriggers.Common.v2;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.OnInsert
{
    public class OnInsertTriggerCondition<TTriggerEntity> : TriggerCondition
        where TTriggerEntity : class
    {
        public OnInsertTriggerCondition(Expression<Func<TTriggerEntity, bool>> condition) 
            : base(condition)
        {
        }

        internal override ArgumentTypes ConditionPrefixes => new()
        {
            [Condition.Parameters[0].Name] = ArgumentType.New,
        };
    }
}
