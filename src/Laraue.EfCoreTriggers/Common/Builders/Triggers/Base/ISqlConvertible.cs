using Laraue.EfCoreTriggers.Common.Builders.Providers;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.Base
{
    internal interface ISqlConvertible
    {
        SqlBuilder BuildSql(ITriggerProvider provider);
    }
}