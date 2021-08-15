using Laraue.EfCoreTriggers.SqlGeneration;

namespace Laraue.EfCoreTriggers.TriggerBuilders.Base
{
    public interface ISqlConvertible
    {
        SqlBuilder BuildSql(ITriggerProvider provider);
    }
}