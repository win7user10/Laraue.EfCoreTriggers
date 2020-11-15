using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Laraue.EfCoreTriggers.Common.Builders.Providers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.OnDelete
{
    public class OnDeleteTriggerInsertAction<TTriggerEntity, TInsertEntity> : TriggerInsertAction<TTriggerEntity, TInsertEntity>
        where TTriggerEntity : class
        where TInsertEntity : class
    {
        public OnDeleteTriggerInsertAction(Expression<Func<TTriggerEntity, TInsertEntity>> setValues)
            : base(setValues)
        {
        }

        internal override Dictionary<string, ArgumentType> InsertExpressionPrefixes => new Dictionary<string, ArgumentType>
        {
            [InsertExpression.Parameters[0].Name] = ArgumentType.Old,
        };
    }
}