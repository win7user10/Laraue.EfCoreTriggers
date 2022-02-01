using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Laraue.EfCoreTriggers.Common.v2;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.OnUpdate
{
    public class OnUpdateTriggerUpdateAction<TTriggerEntity, TUpdateEntity> : TriggerUpdateAction
        where TTriggerEntity : class
        where TUpdateEntity : class
    {
        public OnUpdateTriggerUpdateAction(
            Expression<Func<TTriggerEntity, TTriggerEntity, TUpdateEntity, bool>> setFilter,
            Expression<Func<TTriggerEntity, TTriggerEntity, TUpdateEntity, TUpdateEntity>> setValues)
                : base (setFilter, setValues)
        {
        }

        internal override ArgumentTypes UpdateFilterPrefixes => new()
        {
            [UpdateFilter.Parameters[0].Name] = ArgumentType.Old,
            [UpdateFilter.Parameters[1].Name] = ArgumentType.New,
        };

        internal override ArgumentTypes UpdateExpressionPrefixes => new()
        {
            [UpdateExpression.Parameters[0].Name] = ArgumentType.Old,
            [UpdateExpression.Parameters[1].Name] = ArgumentType.New,
        };
    }
}