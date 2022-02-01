﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Laraue.EfCoreTriggers.Common.v2;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.OnDelete
{
    public class OnDeleteTriggerDeleteAction<TTriggerEntity, TDeleteEntity> : TriggerDeleteAction
        where TTriggerEntity : class
        where TDeleteEntity : class
    {
        public OnDeleteTriggerDeleteAction(Expression<Func<TTriggerEntity, TDeleteEntity, bool>> deleteFilter)
            : base (deleteFilter)
        {
        }

        internal override ArgumentTypes DeleteFilterPrefixes => new()
        {
            [DeletePredicate.Parameters[0].Name] = ArgumentType.Old,
        };
    }
}