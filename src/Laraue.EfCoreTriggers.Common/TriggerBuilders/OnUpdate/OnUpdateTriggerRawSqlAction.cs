using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Laraue.EfCoreTriggers.Common.v2;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.OnUpdate
{
    public class OnUpdateTriggerRawSqlAction<TTriggerEntity> : TriggerRawAction
        where TTriggerEntity : class
    {
        public OnUpdateTriggerRawSqlAction(string sql, params Expression<Func<TTriggerEntity, TTriggerEntity, object>>[] argumentSelectors)
            : base (sql, argumentSelectors)
        {
        }

        protected override ArgumentTypes GetArgumentPrefixes(ReadOnlyCollection<ParameterExpression> parameters)
        {
            return new ArgumentTypes
            {
                [parameters[0].Name] = ArgumentType.Old,
                [parameters[1].Name] = ArgumentType.New,
            };
        }
    }
}