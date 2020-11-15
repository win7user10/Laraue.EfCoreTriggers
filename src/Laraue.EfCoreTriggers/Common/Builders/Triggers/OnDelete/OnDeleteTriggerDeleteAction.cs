using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Laraue.EfCoreTriggers.Common.Builders.Providers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.OnDelete
{
    public class OnDeleteTriggerDeleteAction<TTriggerEntity, TDeleteEntity> : TriggerDeleteAction<TTriggerEntity, TDeleteEntity>
        where TTriggerEntity : class
        where TDeleteEntity : class
    {
        public OnDeleteTriggerDeleteAction(Expression<Func<TTriggerEntity, TDeleteEntity, bool>> deleteFilter)
            : base (deleteFilter)
        {
        }

        internal override Dictionary<string, ArgumentType> DeleteFilterPrefixes => new Dictionary<string, ArgumentType>
        {
            [DeleteFilter.Parameters[0].Name] = ArgumentType.Old,
        };
    }
}