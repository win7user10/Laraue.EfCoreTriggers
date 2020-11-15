using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Laraue.EfCoreTriggers.Common.Builders.Providers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.OnUpdate
{
    public class OnUpdateTriggerInsertAction<TTriggerEntity, TInsertEntity> : TriggerInsertAction<TTriggerEntity, TInsertEntity>
        where TTriggerEntity : class
        where TInsertEntity : class
    {
        public OnUpdateTriggerInsertAction(Expression<Func<TTriggerEntity, TTriggerEntity, TInsertEntity, TInsertEntity>> setValues)
            : base (setValues)
        {
        }

        internal override Dictionary<string, ArgumentType> InsertExpressionPrefixes => new Dictionary<string, ArgumentType>
        {
            [InsertExpression.Parameters[0].Name] = ArgumentType.Old,
            [InsertExpression.Parameters[1].Name] = ArgumentType.New,
        };
    }
}