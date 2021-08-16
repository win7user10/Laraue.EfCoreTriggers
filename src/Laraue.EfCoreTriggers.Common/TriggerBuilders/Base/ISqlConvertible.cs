using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Base
{
    public interface ISqlConvertible
    {
        SqlBuilder BuildSql(ITriggerProvider provider);
    }
}