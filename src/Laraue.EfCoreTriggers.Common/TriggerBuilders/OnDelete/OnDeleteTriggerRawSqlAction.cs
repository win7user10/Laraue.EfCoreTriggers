using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.OnDelete
{
    public class OnDeleteTriggerRawSqlAction<TTriggerEntity> : TriggerRawAction<TTriggerEntity>
        where TTriggerEntity : class
    {
        public OnDeleteTriggerRawSqlAction(string sql, params Expression<Func<TTriggerEntity, object>>[] argumentSelectors)
            : base (sql, argumentSelectors)
        {
        }

        protected override Dictionary<string, ArgumentType> GetArgumentPrefixes(ReadOnlyCollection<ParameterExpression> parameters)
        {
            return new Dictionary<string, ArgumentType>
            {
                [parameters[0].Name] = ArgumentType.Old
            };
        }
    }
}