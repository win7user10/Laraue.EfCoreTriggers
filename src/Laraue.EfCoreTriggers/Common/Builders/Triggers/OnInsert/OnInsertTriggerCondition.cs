using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Laraue.EfCoreTriggers.Common.Builders.Visitor;
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

        public override Dictionary<string, ArgumentPrefix> ConditionPrefixes => new Dictionary<string, ArgumentPrefix>
        {
            [Condition.Parameters[0].Name] = ArgumentPrefix.New,
        };
    }
}
