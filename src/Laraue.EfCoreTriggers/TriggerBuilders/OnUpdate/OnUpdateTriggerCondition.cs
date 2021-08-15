using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.TriggerBuilders.OnUpdate
{
    public class OnUpdateTriggerCondition<TTriggerEntity> : TriggerCondition<TTriggerEntity>
        where TTriggerEntity : class
    {
        public OnUpdateTriggerCondition(Expression<Func<TTriggerEntity, TTriggerEntity, bool>> condition)
            : base(condition)
        {
        }

        internal override Dictionary<string, ArgumentType> ConditionPrefixes => new()
        {
            [Condition.Parameters[0].Name] = ArgumentType.Old,
            [Condition.Parameters[1].Name] = ArgumentType.New,
        };
    }
}
