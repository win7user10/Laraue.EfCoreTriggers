using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Laraue.EfCoreTriggers.Common.v2;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.OnUpdate
{
    public class OnUpdateTriggerDeleteAction<TTriggerEntity, TUpdateEntity> : TriggerDeleteAction
        where TTriggerEntity : class
        where TUpdateEntity : class
    {
        public OnUpdateTriggerDeleteAction(Expression<Func<TTriggerEntity, TTriggerEntity, TUpdateEntity, bool>> deleteFilter)
            : base (deleteFilter)
        {
        }

        internal override ArgumentTypes DeleteFilterPrefixes => new()
        {
            [DeletePredicate.Parameters[0].Name] = ArgumentType.Old,
            [DeletePredicate.Parameters[1].Name] = ArgumentType.New,
        };
    }
}