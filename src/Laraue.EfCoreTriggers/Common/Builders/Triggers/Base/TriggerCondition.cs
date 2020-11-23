using Laraue.EfCoreTriggers.Common.Builders.Providers;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.Base
{
    public abstract class TriggerCondition<TTriggerEntity> : ISqlConvertible
        where TTriggerEntity : class
    {
        internal LambdaExpression Condition { get; }

        public TriggerCondition(LambdaExpression triggerCondition)
            => Condition = triggerCondition;

        public virtual SqlBuilder BuildSql(ITriggerProvider visitor)
            => visitor.GetTriggerConditionSql(this);

        internal abstract Dictionary<string, ArgumentType> ConditionPrefixes { get; }
    }
}