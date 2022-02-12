using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Base
{
    public class TriggerUpdateAction : ITriggerAction
    {
        internal LambdaExpression UpdateFilter;
        internal LambdaExpression UpdateExpression;

        internal TriggerUpdateAction(
            LambdaExpression updateFilter,
            LambdaExpression updateExpression)
        {
            UpdateFilter = updateFilter;
            UpdateExpression = updateExpression;
        }

        internal virtual ArgumentTypes UpdateFilterPrefixes { get; }

        internal virtual ArgumentTypes UpdateExpressionPrefixes { get; }
        
        public Type GetEntityType()
        {
            return UpdateExpression.Parameters[0].GetType();
        }
    }
}