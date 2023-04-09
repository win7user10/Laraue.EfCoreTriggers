using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Abstractions;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Actions
{
    public sealed class TriggerInsertAction : ITriggerAction
    {
        internal LambdaExpression InsertExpression;

        public TriggerInsertAction(LambdaExpression insertExpression)
        {
            InsertExpression = insertExpression;
        }
    }
}