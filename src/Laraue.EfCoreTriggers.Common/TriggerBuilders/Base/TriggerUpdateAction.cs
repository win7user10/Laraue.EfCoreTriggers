using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.v2;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Base
{
    public abstract class TriggerUpdateAction : ITriggerAction
    {
        internal LambdaExpression UpdateFilter;
        internal LambdaExpression UpdateExpression;

        protected TriggerUpdateAction(
            LambdaExpression updateFilter,
            LambdaExpression updateExpression)
        {
            UpdateFilter = updateFilter;
            UpdateExpression = updateExpression;
        }

        internal abstract ArgumentTypes UpdateFilterPrefixes { get; }

        internal abstract ArgumentTypes UpdateExpressionPrefixes { get; }
        
        public Type GetEntityType()
        {
            return UpdateExpression.Parameters[0].GetType();
        }
    }
}