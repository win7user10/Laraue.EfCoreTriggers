using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.OnInsert
{
    public class OnInsertTriggerUpdateAction<TTriggerEntity, TUpdateEntity> : TriggerUpdateAction
        where TTriggerEntity : class
        where TUpdateEntity : class
    {
        public OnInsertTriggerUpdateAction(
            Expression<Func<TTriggerEntity, TUpdateEntity, bool>> setFilter,
            Expression<Func<TTriggerEntity, TUpdateEntity, TUpdateEntity>> setValues)
                : base (setFilter, setValues)
        {
        }

        internal override ArgumentTypes UpdateFilterPrefixes => new()
        {
            [UpdateFilter.Parameters[0].Name] = ArgumentType.New,
        };

        internal override ArgumentTypes UpdateExpressionPrefixes => new()
        {
            [UpdateExpression.Parameters[0].Name] = ArgumentType.New,
        };
    }
}
