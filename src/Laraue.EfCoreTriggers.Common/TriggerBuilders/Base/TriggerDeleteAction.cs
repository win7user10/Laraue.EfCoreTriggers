using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.v2;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Base
{
    public abstract class TriggerDeleteAction : ITriggerAction
    {
        /// <summary>
        /// Expression to delete, e.g. Users.Where(x => x.Id == 2)
        /// </summary>
        internal LambdaExpression DeletePredicate;

        protected TriggerDeleteAction(LambdaExpression deletePredicate)
        {
            DeletePredicate = deletePredicate;
        }

        internal abstract ArgumentTypes DeleteFilterPrefixes { get; }
        
        public Type GetEntityType()
        {
            return DeletePredicate.Parameters[0].GetType();
        }
    }
}