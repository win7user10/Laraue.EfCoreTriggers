using Laraue.EfCoreTriggers.Common.Builders.Providers;
using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.OnUpdate
{
    public class OnUpdateTriggerDeleteAction<TTriggerEntity, TUpdateEntity> : TriggerDeleteAction<TTriggerEntity, TUpdateEntity>
        where TTriggerEntity : class
        where TUpdateEntity : class
    {
        public OnUpdateTriggerDeleteAction(Expression<Func<TTriggerEntity, TTriggerEntity, TUpdateEntity, bool>> deleteFilter)
            : base (deleteFilter)
        {
        }

        internal override Dictionary<string, ArgumentType> DeleteFilterPrefixes => new Dictionary<string, ArgumentType>
        {
            [DeleteFilter.Parameters[0].Name] = ArgumentType.Old,
            [DeleteFilter.Parameters[1].Name] = ArgumentType.New,
        };
    }
}