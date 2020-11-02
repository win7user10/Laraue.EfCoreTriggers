using Laraue.EfCoreTriggers.Common.Builders.Visitor;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.Base
{
    public abstract class TriggerCondition : ISqlConvertible
    {
        public Expression Condition { get; }

        public TriggerCondition(Expression triggerCondition)
        {
            Condition = triggerCondition;
        }

        public abstract string BuildSql(ITriggerSqlVisitor visitor);
    }
}