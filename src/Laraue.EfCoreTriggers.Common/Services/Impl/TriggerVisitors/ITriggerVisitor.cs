﻿using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

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
    /// <returns></returns>
    string GenerateDeleteTriggerSql(string triggerName);
}