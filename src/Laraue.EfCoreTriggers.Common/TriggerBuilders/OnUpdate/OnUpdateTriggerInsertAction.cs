﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.OnUpdate
{
    public class OnUpdateTriggerInsertAction<TTriggerEntity, TInsertEntity> : TriggerInsertAction
        where TTriggerEntity : class
        where TInsertEntity : class
    {
        public OnUpdateTriggerInsertAction(Expression<Func<TTriggerEntity, TTriggerEntity, TInsertEntity>> setValues)
            : base (setValues)
        {
        }

        internal override ArgumentTypes InsertExpressionPrefixes => new()
        {
            [InsertExpression.Parameters[0].Name] = ArgumentType.Old,
            [InsertExpression.Parameters[1].Name] = ArgumentType.New,
        };
    }
}