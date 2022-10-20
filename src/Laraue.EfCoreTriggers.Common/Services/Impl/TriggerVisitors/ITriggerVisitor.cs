using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;

/// <summary>
/// Generates create and delete SQL for the trigger.
/// </summary>
public interface ITriggerVisitor
{
    /// <summary>
    /// Generates create trigger SQL.
    /// </summary>
    /// <param name="trigger"></param>
    /// <returns></returns>
    string GenerateCreateTriggerSql(ITrigger trigger);

    /// <summary>
    /// Generates drop trigger SQL.
    /// </summary>
    /// <param name="triggerName"></param>
    /// <param name="entityType"></param>
    /// <returns></returns>
    string GenerateDeleteTriggerSql(string triggerName, IEntityType entityType);
}