using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Laraue.EfCoreTriggers.Common.Builders.Visitor;
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

        internal override Dictionary<string, ArgumentPrefix> DeleteFilterPrefixes => new Dictionary<string, ArgumentPrefix>
        {
            [DeleteFilter.Parameters[0].Name] = ArgumentPrefix.Old,
            [DeleteFilter.Parameters[1].Name] = ArgumentPrefix.New,
        };
    }
}