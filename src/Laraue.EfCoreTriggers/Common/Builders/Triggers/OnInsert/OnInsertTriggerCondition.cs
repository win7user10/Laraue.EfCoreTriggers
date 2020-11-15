using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Laraue.EfCoreTriggers.Common.Builders.Providers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.OnInsert
{
    public class OnInsertTriggerCondition<TTriggerEntity> : TriggerCondition<TTriggerEntity>
        where TTriggerEntity : class
    {
        public OnInsertTriggerCondition(Expression<Func<TTriggerEntity, bool>> condition) : base(condition)
        {
        }

        internal override Dictionary<string, ArgumentType> ConditionPrefixes => new Dictionary<string, ArgumentType>
        {
            [Condition.Parameters[0].Name] = ArgumentType.New,
        };
    }
}
