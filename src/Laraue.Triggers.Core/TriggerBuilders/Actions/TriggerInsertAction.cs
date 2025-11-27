using System.Linq.Expressions;
using Laraue.Triggers.Core.TriggerBuilders.Abstractions;

namespace Laraue.Triggers.Core.TriggerBuilders.Actions
{
    /// <summary>
    /// Trigger insert action.
    /// </summary>
    public sealed class TriggerInsertAction : ITriggerAction
    {
        /// <summary>
        /// The entity should be inserted expression, tableRefs => new User { Age = tableRefs.Old.Age }.
        /// </summary>
        public readonly LambdaExpression InsertExpression;

        /// <summary>
        /// Initializes a new instance of <see cref="TriggerInsertAction"/>.
        /// </summary>
        /// <param name="insertExpression"></param>
        public TriggerInsertAction(LambdaExpression insertExpression)
        {
            InsertExpression = insertExpression;
        }
    }
}