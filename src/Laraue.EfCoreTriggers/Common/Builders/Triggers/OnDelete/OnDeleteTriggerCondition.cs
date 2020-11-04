using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Laraue.EfCoreTriggers.Common.Builders.Visitor;
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

        public override Dictionary<string, ArgumentPrefix> ConditionPrefixes => new Dictionary<string, ArgumentPrefix>
        {
            [Condition.Parameters[0].Name] = ArgumentPrefix.Old,
        };
    }
}
