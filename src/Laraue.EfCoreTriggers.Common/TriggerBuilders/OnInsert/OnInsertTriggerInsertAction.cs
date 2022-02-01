using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Laraue.EfCoreTriggers.Common.v2;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.OnInsert
{
    public class OnInsertTriggerInsertAction<TTriggerEntity, TInsertEntity> : TriggerInsertAction
        where TTriggerEntity : class
        where TInsertEntity : class
    {
        public OnInsertTriggerInsertAction(Expression<Func<TTriggerEntity, TInsertEntity>> setValues)
            : base (setValues)
        {
        }

        internal override ArgumentTypes InsertExpressionPrefixes => new()
        {
            [InsertExpression.Parameters[0].Name] = ArgumentType.New,
        };
    }
}