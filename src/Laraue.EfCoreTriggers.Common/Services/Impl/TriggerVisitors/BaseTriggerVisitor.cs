using System;
using System.Collections.Generic;
using System.Linq;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;

/// <inheritdoc />
public abstract class BaseTriggerVisitor : ITriggerVisitor
{
    /// <summary>
    /// When the trigger can be fired in the current SQL provider.
    /// </summary>
    protected virtual IEnumerable<TriggerTime> AvailableTriggerTimes { get; } = new[]
    {
        TriggerTime.After,
        TriggerTime.Before, 
        TriggerTime.InsteadOf
    };
    
    /// <summary>
    /// How named each of <see cref="TriggerTime"/> in the current SQL provider.
    /// </summary>
    protected virtual Dictionary<TriggerTime, string> TriggerTimeNames { get; } = new()
    {
        [TriggerTime.After] = "AFTER",
        [TriggerTime.Before] = "BEFORE",
        [TriggerTime.InsteadOf] = "INSTEAD OF",
    };

    /// <summary>
    /// Return name for the passed <see cref="TriggerTime"/>.
    /// </summary>
    /// <param name="triggerTime"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    protected string GetTriggerTimeName(TriggerTime triggerTime)
    {
        if (!AvailableTriggerTimes.Contains(triggerTime) ||
            !TriggerTimeNames.TryGetValue(triggerTime, out var triggerTypeName))
        {
            throw new NotSupportedException($"Trigger time {triggerTime} is not supported for {GetType()}.");
        }

        return triggerTypeName;
    }

    /// <inheritdoc />
    public abstract string GenerateCreateTriggerSql(ITrigger trigger);

    /// <inheritdoc />
    public abstract string GenerateDeleteTriggerSql(string triggerName, IEntityType entityType);
}