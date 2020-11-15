using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Laraue.EfCoreTriggers.Common.Builders.Providers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.OnDelete
{
    public class OnDeleteTriggerCondition<TTriggerEntity> : TriggerCondition<TTriggerEntity>
        where TTriggerEntity : class
    {
        public OnDeleteTriggerCondition(Expression<Func<TTriggerEntity, bool>> condition) : base(condition)
        {
        }

        internal override Dictionary<string, ArgumentType> ConditionPrefixes => new Dictionary<string, ArgumentType>
        {
            [Condition.Parameters[0].Name] = ArgumentType.Old,
        };
    }
}
