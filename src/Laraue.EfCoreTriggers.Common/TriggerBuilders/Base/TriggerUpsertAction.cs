using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Services;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Base
{
    public abstract class TriggerUpsertAction : ITriggerAction
    {
        public LambdaExpression MatchExpression;
        public LambdaExpression InsertExpression;
        public LambdaExpression OnMatchExpression;

        protected TriggerUpsertAction(
            LambdaExpression matchExpression,
            LambdaExpression insertExpression,
            LambdaExpression onMatchExpression)
        {
            MatchExpression = matchExpression;
            InsertExpression = insertExpression;
            OnMatchExpression = onMatchExpression;
        }

        public abstract ArgumentTypes InsertExpressionPrefixes { get; }

        public abstract ArgumentTypes OnMatchExpressionPrefixes { get; }

        public abstract ArgumentTypes MatchExpressionPrefixes { get; }
    }
}