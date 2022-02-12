using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.OnDelete
{
    public class OnDeleteTriggerRawSqlAction<TTriggerEntity> : TriggerRawAction
        where TTriggerEntity : class
    {
        public OnDeleteTriggerRawSqlAction(string sql, params Expression<Func<TTriggerEntity, object>>[] argumentSelectors)
            : base (sql, argumentSelectors)
        {
        }

        protected override ArgumentTypes GetArgumentPrefixes(ReadOnlyCollection<ParameterExpression> parameters)
        {
            return new ArgumentTypes
            {
                [parameters[0].Name] = ArgumentType.Old
            };
        }
    }
}