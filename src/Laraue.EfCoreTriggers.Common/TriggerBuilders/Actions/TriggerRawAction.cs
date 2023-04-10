using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Abstractions;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Actions
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TriggerRawAction : ITriggerAction
    {
        internal readonly LambdaExpression[] ArgumentSelectorExpressions;
        
        internal readonly string Sql;

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