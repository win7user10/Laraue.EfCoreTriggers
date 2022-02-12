using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

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
