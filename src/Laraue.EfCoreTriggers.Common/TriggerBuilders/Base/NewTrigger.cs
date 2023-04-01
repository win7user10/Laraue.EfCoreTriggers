using System;
using System.Collections.Generic;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

public sealed class NewTrigger<TTriggerEntity, TTriggerEntityRefs> : ITrigger
    where TTriggerEntity : class
    where TTriggerEntityRefs : Refs<TTriggerEntity>
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
        Action<NewTriggerActions<TTriggerEntity, TTriggerEntityRefs>> triggerAction)
    {
        var actions = new NewTriggerActions<TTriggerEntity, TTriggerEntityRefs>();

        triggerAction(actions);

        return this;
    }
}