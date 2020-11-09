using Laraue.EfCoreTriggers.Common.Builders.Visitor;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.Base
{
    internal interface ISqlConvertible
    {
        string BuildSql(ITriggerSqlVisitor visitor);
    }
}