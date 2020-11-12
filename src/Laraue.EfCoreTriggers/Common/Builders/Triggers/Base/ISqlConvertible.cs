using Laraue.EfCoreTriggers.Common.Builders.Visitor;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.Base
{
    internal interface ISqlConvertible
    {
        GeneratedSql BuildSql(ITriggerSqlVisitor visitor);
    }
}