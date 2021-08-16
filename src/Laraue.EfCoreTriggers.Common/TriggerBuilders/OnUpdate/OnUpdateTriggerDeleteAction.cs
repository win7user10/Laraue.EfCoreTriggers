using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.OnUpdate
{
    public class OnUpdateTriggerDeleteAction<TTriggerEntity, TUpdateEntity> : TriggerDeleteAction<TTriggerEntity, TUpdateEntity>
        where TTriggerEntity : class
        where TUpdateEntity : class
    {
        public OnUpdateTriggerDeleteAction(Expression<Func<TTriggerEntity, TTriggerEntity, TUpdateEntity, bool>> deleteFilter)
            : base (deleteFilter)
        {
        }

        internal override Dictionary<string, ArgumentType> DeleteFilterPrefixes => new()
        {
            [DeleteFilter.Parameters[0].Name] = ArgumentType.Old,
            [DeleteFilter.Parameters[1].Name] = ArgumentType.New,
        };
    }
}