using System;
using System.Collections.Generic;
using Laraue.Linq2Triggers.Core.TriggerBuilders.Abstractions;
using Laraue.Linq2Triggers.Core.TriggerBuilders.Actions;
using Laraue.Linq2Triggers.Core.TriggerBuilders.TableRefs;

namespace Laraue.Linq2Triggers.Core.TriggerBuilders
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

        /// <inheritdoc />
        public string Name { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="Trigger{TTriggerEntity,TTriggerEntityRefs}"/>.
        /// </summary>
        /// <param name="triggerEvent"></param>
        /// <param name="triggerTime"></param>
        public Trigger(TriggerEvent triggerEvent, TriggerTime triggerTime)
        {
            TriggerTime = triggerTime;
            TriggerEvent = triggerEvent;

            var triggerName = Constants.GetTriggerName(triggerTime, triggerEvent, typeof(TTriggerEntity));
            Name = GetFullTriggerName(triggerName);
        }
        
        /// <summary>
        /// Sets the trigger name. The full name will consist of concatenation of 
        /// <see cref="Constants.AnnotationKey"/> and this string.
        /// </summary>
        public Trigger<TTriggerEntity, TTriggerEntityRefs> SetTriggerName(string name)
        {
            Name = GetFullTriggerName(name);

            return this;
        }

        /// <summary>
        /// Creates a new triggers action group. Each action group represents the separated trigger.
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

        private string GetFullTriggerName(string triggerName)
        {
            return $"{Constants.AnnotationKey}{triggerName}";
        }
    }
}