using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Laraue.EfCoreTriggers.Common.Builders.Providers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.OnDelete
{
    public class OnDeleteTriggerUpdateAction<TTriggerEntity, TUpdateEntity> : TriggerUpdateAction<TTriggerEntity, TUpdateEntity>
        where TTriggerEntity : class
        where TUpdateEntity : class
    {
        public OnDeleteTriggerUpdateAction(
            Expression<Func<TTriggerEntity, TUpdateEntity, bool>> setFilter,
            Expression<Func<TTriggerEntity, TUpdateEntity, TUpdateEntity>> setValues)
                : base (setFilter, setValues)
        {
        }

        internal override Dictionary<string, ArgumentType> UpdateFilterPrefixes => new Dictionary<string, ArgumentType>
        {
            [UpdateFilter.Parameters[0].Name] = ArgumentType.Old, 
        };

        internal override Dictionary<string, ArgumentType> UpdateExpressionPrefixes => new Dictionary<string, ArgumentType>
        {
            [UpdateExpression.Parameters[0].Name] = ArgumentType.Old,
        };
    }
}