using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Laraue.EfCoreTriggers.Common.Builders.Visitor;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.OnInsert
{
    public class OnInsertTriggerUpdateAction<TTriggerEntity, TUpdateEntity> : TriggerUpdateAction<TTriggerEntity, TUpdateEntity>
        where TTriggerEntity : class
        where TUpdateEntity : class
    {
        public OnInsertTriggerUpdateAction(
            Expression<Func<TTriggerEntity, TUpdateEntity, bool>> setFilter,
            Expression<Func<TTriggerEntity, TUpdateEntity, TUpdateEntity>> setValues)
                : base (setFilter, setValues)
        {
        }

        internal override Dictionary<string, ArgumentPrefix> UpdateFilterPrefixes => new Dictionary<string, ArgumentPrefix>
        {
            [UpdateFilter.Parameters[0].Name] = ArgumentPrefix.New,
        };

        internal override Dictionary<string, ArgumentPrefix> UpdateExpressionPrefixes => new Dictionary<string, ArgumentPrefix>
        {
            [UpdateExpression.Parameters[0].Name] = ArgumentPrefix.New,
        };
    }
}
