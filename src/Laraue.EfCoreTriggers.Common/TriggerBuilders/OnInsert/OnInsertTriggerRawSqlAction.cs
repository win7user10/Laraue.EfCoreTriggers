using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Laraue.EfCoreTriggers.Common.v2;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.OnInsert
{
    public class OnInsertTriggerRawSqlAction<TTriggerEntity> : TriggerRawAction
        where TTriggerEntity : class
    {
        public OnInsertTriggerRawSqlAction(string sql, params Expression<Func<TTriggerEntity, object>>[] argumentSelectors)
            : base (sql, argumentSelectors)
        {
        }

        protected override ArgumentTypes GetArgumentPrefixes(ReadOnlyCollection<ParameterExpression> parameters)
        {
            return new ArgumentTypes
            {
                [parameters[0].Name] = ArgumentType.New
            };
        }
    }
}