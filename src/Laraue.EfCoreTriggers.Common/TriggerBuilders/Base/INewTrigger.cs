using System;
using System.Collections.Generic;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

/// <summary>
/// One of the database triggers.
/// </summary>
public interface INewTrigger
{
    /// <summary>
    /// What should be happened when trigger fires.
    /// </summary>
    IList<NewTriggerAction> Actions { get; }
        
    /// <summary>
    /// The action when the trigger should be fired.
    /// </summary>
    TriggerEvent TriggerEvent { get; }
        
    /// <summary>
    /// The moment to fire trigger.
    /// </summary>
    TriggerTime TriggerTime { get; }

    /// <summary>
    /// Entity CLR type the trigger belongs to.
    /// </summary>
    Type TriggerEntityType { get; }

    /// <summary>
    /// Trigger name.
    /// </summary>
    string Name { get; }
}