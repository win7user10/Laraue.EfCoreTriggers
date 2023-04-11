using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Abstractions;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Actions
{
    /// <summary>
    /// Trigger raw action.
    /// </summary>
    public sealed class TriggerRawAction : ITriggerAction
    {
        /// <summary>
        /// Member expressions that should be converted to strings
        /// e.g. tableRefs => tableRefs.Old.Age
        /// </summary>
        public readonly LambdaExpression[] ArgumentSelectorExpressions;
        
        /// <summary>
        /// Sql expression. Can take {0}, {1} arguments. These arguments
        /// will be formatted based on <see cref="ArgumentSelectorExpressions"/>.
        /// </summary>
        public readonly string Sql;

        /// <summary>
        /// Initializes a new instance of <see cref="TriggerRawAction"/>.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="argumentSelectors"></param>
        public TriggerRawAction(string sql, params LambdaExpression[] argumentSelectors)
        {
            ArgumentSelectorExpressions = argumentSelectors;
            Sql = sql;
        }
    }
}