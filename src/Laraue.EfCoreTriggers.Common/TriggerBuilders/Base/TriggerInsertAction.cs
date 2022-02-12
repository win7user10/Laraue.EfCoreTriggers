using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Services;

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