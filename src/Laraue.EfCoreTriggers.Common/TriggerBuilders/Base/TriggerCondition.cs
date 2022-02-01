using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.v2;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Base
{
    public class TriggerCondition : ITriggerAction
    {
        /// <summary>
        /// Expression to delete, e.g. Users.Where(x => x.Id == 2)
        /// </summary>
        internal LambdaExpression Condition;
        
        protected TriggerCondition(LambdaExpression condition)
        {
            Condition = condition;
        }
        
        internal TriggerCondition(LambdaExpression condition, ArgumentTypes conditionPrefixes)
            : this(condition)
        {
            ConditionPrefixes = conditionPrefixes;
        }

        internal virtual ArgumentTypes ConditionPrefixes { get; }
        
        public Type GetEntityType()
        {
            return Condition.Parameters[0].GetType();
        }
    }
}