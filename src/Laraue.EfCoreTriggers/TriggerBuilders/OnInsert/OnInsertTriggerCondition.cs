using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.TriggerBuilders.OnInsert
{
    public class OnInsertTriggerCondition<TTriggerEntity> : TriggerCondition<TTriggerEntity>
        where TTriggerEntity : class
    {
        public OnInsertTriggerCondition(Expression<Func<TTriggerEntity, bool>> condition) : base(condition)
        {
        }

        internal override Dictionary<string, ArgumentType> ConditionPrefixes => new()
        {
            [Condition.Parameters[0].Name] = ArgumentType.New,
        };
    }
}
