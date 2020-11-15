using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Laraue.EfCoreTriggers.Common.Builders.Providers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.OnInsert
{
    public class OnInsertTriggerInsertAction<TTriggerEntity, TInsertEntity> : TriggerInsertAction<TTriggerEntity, TInsertEntity>
        where TTriggerEntity : class
        where TInsertEntity : class
    {
        public OnInsertTriggerInsertAction(Expression<Func<TTriggerEntity, TInsertEntity>> setValues)
            : base (setValues)
        {
        }

        internal override Dictionary<string, ArgumentType> InsertExpressionPrefixes => new Dictionary<string, ArgumentType>
        {
            [InsertExpression.Parameters[0].Name] = ArgumentType.New,
        };
    }
}