using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Laraue.EfCoreTriggers.Common.v2;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.OnDelete
{
    public class OnDeleteTriggerInsertAction<TTriggerEntity, TInsertEntity> : TriggerInsertAction
        where TTriggerEntity : class
        where TInsertEntity : class
    {
        public OnDeleteTriggerInsertAction(Expression<Func<TTriggerEntity, TInsertEntity>> setValues)
            : base(setValues)
        {
        }

        internal override ArgumentTypes InsertExpressionPrefixes => new()
        {
            [InsertExpression.Parameters[0].Name] = ArgumentType.Old,
        };
    }
}