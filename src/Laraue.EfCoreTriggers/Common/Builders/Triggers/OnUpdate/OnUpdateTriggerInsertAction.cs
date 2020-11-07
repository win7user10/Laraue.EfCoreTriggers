using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Laraue.EfCoreTriggers.Common.Builders.Visitor;
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

        public override Dictionary<string, ArgumentPrefix> InsertExpressionPrefixes => new Dictionary<string, ArgumentPrefix>
        {
            [InsertExpression.Parameters[0].Name] = ArgumentPrefix.Old,
            [InsertExpression.Parameters[1].Name] = ArgumentPrefix.New,
        };
    }
}