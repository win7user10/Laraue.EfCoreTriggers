using System;
using System.Collections.Generic;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Abstractions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Actions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders
{
    /// <inheritdoc />
    public sealed class Trigger<TTriggerEntity, TTriggerEntityRefs> : ITrigger
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
        public IList<TriggerActionsGroup> Actions { get; } = new List<TriggerActionsGroup>();

        /// <summary>
        /// Initializes a new instance of <see cref="Trigger{TTriggerEntity,TTriggerEntityRefs}"/>.
        /// </summary>
        /// <param name="triggerEvent"></param>
        /// <param name="triggerTime"></param>
        public Trigger(TriggerEvent triggerEvent, TriggerTime triggerTime)
        {
            TriggerTime = triggerTime;
            TriggerEvent = triggerEvent;
        }

        /// <inheritdoc />
        public string Name
            => $"{Constants.AnnotationKey}_{TriggerTime}_{TriggerEvent}_{typeof(TTriggerEntity).Name}".ToUpper();

        /// <summary>
        /// Creates a new triggers action group. Each action groups represents the separated trigger.
        /// </summary>
        /// <param name="triggerAction"></param>
        /// <returns></returns>
        public Trigger<TTriggerEntity, TTriggerEntityRefs> Action(
            Action<TriggerActionsGroup<TTriggerEntity, TTriggerEntityRefs>> triggerAction)
        {
            var action = new TriggerActionsGroup<TTriggerEntity, TTriggerEntityRefs>();

            triggerAction(action);
        
            Actions.Add(action);

            return this;
        }
    }
}