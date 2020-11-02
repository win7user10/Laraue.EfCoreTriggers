using Laraue.EfCoreTriggers.Common.Builders.Visitor;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.Base
{
    public interface ISqlConvertible
    {
        public string BuildSql(ITriggerSqlVisitor visitor);
    }
}