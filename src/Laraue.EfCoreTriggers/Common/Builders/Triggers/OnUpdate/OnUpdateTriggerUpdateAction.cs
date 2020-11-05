using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Laraue.EfCoreTriggers.Common.Builders.Visitor;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.OnUpdate
{
    public class OnUpdateTriggerUpdateAction<TTriggerEntity, TUpdateEntity> : TriggerUpdateAction<TTriggerEntity, TUpdateEntity>
        where TTriggerEntity : class
        where TUpdateEntity : class
    {
        public OnUpdateTriggerUpdateAction(
            Expression<Func<TTriggerEntity, TTriggerEntity, TUpdateEntity, bool>> setFilter,
            Expression<Func<TTriggerEntity, TTriggerEntity, TUpdateEntity, TUpdateEntity>> setValues)
                : base (setFilter, setValues)
        {
        }

        public override Dictionary<string, ArgumentPrefix> UpdateFilterPrefixes => new Dictionary<string, ArgumentPrefix>
        {
            [UpdateFilter.Parameters[0].Name] = ArgumentPrefix.Old,
            [UpdateFilter.Parameters[1].Name] = ArgumentPrefix.New,
        };

        public override Dictionary<string, ArgumentPrefix> UpdateExpressionPrefixes => new Dictionary<string, ArgumentPrefix>
        {
            [UpdateExpression.Parameters[0].Name] = ArgumentPrefix.Old,
            [UpdateExpression.Parameters[1].Name] = ArgumentPrefix.New,
        };
    }
}