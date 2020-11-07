using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Laraue.EfCoreTriggers.Common.Builders.Visitor;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.OnInsert
{
    public class OnInsertTriggerDeleteAction<TTriggerEntity, TDeleteEntity> : TriggerDeleteAction<TTriggerEntity, TDeleteEntity>
        where TTriggerEntity : class
        where TDeleteEntity : class
    {
        public OnInsertTriggerDeleteAction(Expression<Func<TTriggerEntity, TDeleteEntity, bool>> deleteFilter)
            : base (deleteFilter)
        {
        }

        public override Dictionary<string, ArgumentPrefix> DeleteFilterPrefixes => new Dictionary<string, ArgumentPrefix>
        {
            [DeleteFilter.Parameters[0].Name] = ArgumentPrefix.New,
        };
    }
}
