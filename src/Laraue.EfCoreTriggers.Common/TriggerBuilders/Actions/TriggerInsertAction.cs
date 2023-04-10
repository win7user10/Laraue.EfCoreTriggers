using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Abstractions;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Actions
{
    public sealed class TriggerInsertAction : ITriggerAction
    {
        internal readonly LambdaExpression InsertExpression;

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