using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Laraue.EfCoreTriggers.Common.Builders.Providers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.OnUpdate
{
    public class OnUpdateTriggerCondition<TTriggerEntity> : TriggerCondition<TTriggerEntity>
        where TTriggerEntity : class
    {
        public OnUpdateTriggerCondition(Expression<Func<TTriggerEntity, TTriggerEntity, bool>> condition)
            : base(condition)
        {
        }

        internal override Dictionary<string, ArgumentType> ConditionPrefixes => new Dictionary<string, ArgumentType>
        {
            [Condition.Parameters[0].Name] = ArgumentType.Old,
            [Condition.Parameters[1].Name] = ArgumentType.New,
        };
    }
}
