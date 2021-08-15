using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.TriggerBuilders.OnUpdate
{
    public class OnUpdateTriggerUpdateAction<TTriggerEntity, TUpdateEntity> : TriggerUpdateAction<TTriggerEntity, TUpdateEntity>
        where TTriggerEntity : class
        where TUpdateEntity : class
    {
        public OnUpdateTriggerUpdateAction(
            Expression<Func<TTriggerEntity, TTriggerEntity, TUpdateEntity, bool>> setFilter,
            Expression<Func<TTriggerEntity, TTriggerEntity, TUpdateEntity, TUpdateEntity>> setValues)
                : base (setFilter, setValues)
        {
        }

        internal override Dictionary<string, ArgumentType> UpdateFilterPrefixes => new()
        {
            [UpdateFilter.Parameters[0].Name] = ArgumentType.Old,
            [UpdateFilter.Parameters[1].Name] = ArgumentType.New,
        };

        internal override Dictionary<string, ArgumentType> UpdateExpressionPrefixes => new()
        {
            [UpdateExpression.Parameters[0].Name] = ArgumentType.Old,
            [UpdateExpression.Parameters[1].Name] = ArgumentType.New,
        };
    }
}