using Laraue.EfCoreTriggers.Common.Builders.Visitor;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.Base
{
    public abstract class TriggerCondition<TTriggerEntity> : ISqlConvertible
        where TTriggerEntity : class
    {
        public LambdaExpression Condition { get; }

        public TriggerCondition(LambdaExpression triggerCondition)
            => Condition = triggerCondition;

        public virtual string BuildSql(ITriggerSqlVisitor visitor)
            => visitor.GetTriggerConditionSql(this);

        public abstract Dictionary<string, ArgumentPrefix> ConditionPrefixes { get; }
    }
}