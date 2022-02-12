using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;

/// <summary>
/// Generates SQL for all trigger actions and conditions.
/// </summary>
public interface ITriggerVisitor
{
    string GenerateCreateTriggerSql(ITrigger trigger);
    
    string GenerateDeleteTriggerSql(string triggerName);
}