using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

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
    }
}