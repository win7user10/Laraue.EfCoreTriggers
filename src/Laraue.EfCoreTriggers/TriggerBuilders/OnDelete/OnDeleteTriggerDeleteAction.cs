using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.TriggerBuilders.OnDelete
{
    public class OnDeleteTriggerDeleteAction<TTriggerEntity, TDeleteEntity> : TriggerDeleteAction<TTriggerEntity, TDeleteEntity>
        where TTriggerEntity : class
        where TDeleteEntity : class
    {
        public OnDeleteTriggerDeleteAction(Expression<Func<TTriggerEntity, TDeleteEntity, bool>> deleteFilter)
            : base (deleteFilter)
        {
        }

        internal override Dictionary<string, ArgumentType> DeleteFilterPrefixes => new()
        {
            [DeleteFilter.Parameters[0].Name] = ArgumentType.Old,
        };
    }
}