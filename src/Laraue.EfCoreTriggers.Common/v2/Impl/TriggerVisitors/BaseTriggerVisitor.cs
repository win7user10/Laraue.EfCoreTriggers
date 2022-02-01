using System;
using System.Collections.Generic;
using System.Linq;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;

public abstract class BaseTriggerVisitor : ITriggerVisitor
{
    protected virtual IEnumerable<TriggerTime> AvailableTriggerTimes { get; } = new[]
    {
        TriggerTime.After,
        TriggerTime.Before, 
        TriggerTime.InsteadOf
    };
    
    protected virtual Dictionary<TriggerTime, string> TriggerTimeNames { get; } = new()
    {
        [TriggerTime.After] = "AFTER",
        [TriggerTime.Before] = "BEFORE",
        [TriggerTime.InsteadOf] = "INSTEAD OF",
    };

    protected string GetTriggerTimeName(TriggerTime triggerTime)
    {
        if (!AvailableTriggerTimes.Contains(triggerTime) ||
            !TriggerTimeNames.TryGetValue(triggerTime, out var triggerTypeName))
        {
            throw new NotSupportedException($"Trigger time {triggerTime} is not supported for {GetType()}.");
        }

        return triggerTypeName;
    }

    public abstract string GenerateCreateTriggerSql(ITrigger trigger);

    public abstract string GenerateDeleteTriggerSql(string triggerName);
}