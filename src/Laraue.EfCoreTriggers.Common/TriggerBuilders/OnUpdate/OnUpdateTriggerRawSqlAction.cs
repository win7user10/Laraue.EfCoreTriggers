using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.OnUpdate
{
    public class OnUpdateTriggerRawSqlAction<TTriggerEntity> : TriggerRawAction<TTriggerEntity>
        where TTriggerEntity : class
    {
        public OnUpdateTriggerRawSqlAction(string sql, params Expression<Func<TTriggerEntity, TTriggerEntity, object>>[] argumentSelectors)
            : base (sql, argumentSelectors)
        {
        }

        protected override Dictionary<string, ArgumentType> GetArgumentPrefixes(ReadOnlyCollection<ParameterExpression> parameters)
        {
            return new Dictionary<string, ArgumentType>
            {
                [parameters[0].Name] = ArgumentType.Old,
                [parameters[1].Name] = ArgumentType.New,
            };
        }
    }
}