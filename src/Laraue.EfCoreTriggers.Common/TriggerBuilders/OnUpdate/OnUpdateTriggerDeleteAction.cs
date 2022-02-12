using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

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