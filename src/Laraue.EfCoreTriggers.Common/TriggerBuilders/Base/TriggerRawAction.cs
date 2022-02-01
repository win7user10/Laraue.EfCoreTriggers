using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.v2;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Base
{
    public abstract class TriggerRawAction : ITriggerAction
    {
        internal readonly LambdaExpression[] ArgumentSelectorExpressions;
        
        internal readonly string Sql;

        protected TriggerRawAction(string sql, IEnumerable<LambdaExpression> argumentSelectors)
        {
            ArgumentSelectorExpressions = argumentSelectors.ToArray();
            Sql = sql;
        }

        internal ArgumentTypes[] ArgumentPrefixes =>
            ArgumentSelectorExpressions.Select((x, i) => GetArgumentPrefixes(ArgumentSelectorExpressions[i].Parameters)).ToArray();

        protected abstract ArgumentTypes GetArgumentPrefixes(
            ReadOnlyCollection<ParameterExpression> parameters);
        
        public Type GetEntityType()
        {
            return this.GetType();
        }
    }
}