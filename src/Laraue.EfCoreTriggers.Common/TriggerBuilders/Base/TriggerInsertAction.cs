using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.v2;

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
        
        public Type GetEntityType()
        {
            return InsertExpression.Parameters[0].GetType();
        }
    }
}