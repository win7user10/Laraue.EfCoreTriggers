using Laraue.EfCoreTriggers.Common.Builders.Visitor;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.Base
{
    public abstract class TriggerUpdateAction : ISqlConvertible
    {
        public Expression UpdateFilter;
        public Expression UpdateExpression;

        public TriggerUpdateAction(
            Expression updateFilter,
            Expression updateExpression)
        {
            UpdateFilter = updateFilter;
            UpdateExpression = updateExpression;
        }

        public abstract string BuildSql(ITriggerSqlVisitor visitor);
    }
}
