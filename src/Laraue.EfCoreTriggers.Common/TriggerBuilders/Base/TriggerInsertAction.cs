using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Base
{
    public abstract class TriggerInsertAction : ITriggerAction
    {
        internal LambdaExpression InsertExpression;

        protected TriggerInsertAction(LambdaExpression insertExpression)
        {
            InsertExpression = insertExpression;
        }

        internal abstract ArgumentTypes InsertExpressionPrefixes { get; }
    }
}