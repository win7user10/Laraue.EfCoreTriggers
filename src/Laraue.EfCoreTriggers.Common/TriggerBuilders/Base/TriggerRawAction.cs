using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Base
{
    public abstract class TriggerRawAction<TTriggerEntity> : ITriggerAction
       where TTriggerEntity : class
    {
        internal readonly LambdaExpression[] ArgumentSelectorExpressions;
        
        internal readonly string Sql;

        protected TriggerRawAction(string sql, IEnumerable<LambdaExpression> argumentSelectors)
        {
            ArgumentSelectorExpressions = argumentSelectors.ToArray();
            Sql = sql;
        }

        public virtual SqlBuilder BuildSql(ITriggerProvider visitor)
            => visitor.GetTriggerRawActionSql(this);

        internal Dictionary<string, ArgumentType>[] ArgumentPrefixes =>
            ArgumentSelectorExpressions.Select((x, i) => GetArgumentPrefixes(ArgumentSelectorExpressions[i].Parameters)).ToArray();

        protected abstract Dictionary<string, ArgumentType> GetArgumentPrefixes(
            ReadOnlyCollection<ParameterExpression> parameters);
    }
}