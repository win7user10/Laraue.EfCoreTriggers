using System;
using System.Collections.Generic;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

public sealed class NewTrigger<TTriggerEntity, TTriggerEntityRefs> : ITrigger
    where TTriggerEntity : class
    where TTriggerEntityRefs : ITableRef<TTriggerEntity>
{
    /// <inheritdoc />
    public TriggerEvent TriggerEvent { get; }

    /// <inheritdoc />
    public TriggerTime TriggerTime { get; }

    /// <inheritdoc />
    public Type TriggerEntityType => typeof(TTriggerEntity);

    /// <inheritdoc />
    public IList<ITriggerAction> Actions { get; } = new List<ITriggerAction>();

    /// <inheritdoc />
    public IList<ITriggerAction> Conditions  { get; } = new List<ITriggerAction>();

    public NewTrigger(TriggerEvent triggerEvent, TriggerTime triggerTime)
    {
        TriggerTime = triggerTime;
        TriggerEvent = triggerEvent;
    }

    /// <inheritdoc />
    public string Name
        => $"{Constants.AnnotationKey}_{TriggerTime}_{TriggerEvent}_{typeof(TTriggerEntity).Name}".ToUpper();

    public NewTrigger<TTriggerEntity, TTriggerEntityRefs> Action(
        Action<NewTriggerAction<TTriggerEntity, TTriggerEntityRefs>> triggerAction)
    {
        var action = new NewTriggerAction<TTriggerEntity, TTriggerEntityRefs>();

        triggerAction(action);
        
        Actions.Add(action);

        return this;
    }
}