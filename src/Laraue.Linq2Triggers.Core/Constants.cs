using System;
using Laraue.Linq2Triggers.Core.TriggerBuilders;

namespace Laraue.Linq2Triggers.Core
{
    /// <summary>
    /// Library constants
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// All triggers names starts with this key. Acronym from laraue core trigger.
        /// Note: if triggers already created with one prefix, it's changing will be
        /// a problem, because this prefix is used to find trigger annotations
        /// while migrations generating.
        /// The best way will be to generate a new migrations, manually fix they name
        /// to start from the new <see cref="AnnotationKey"/>, only then change the value.
        /// </summary>
        public static string AnnotationKey { get; set; } = "LC_TRIGGER_";
        
        /// <summary>
        /// The function that is used to generate trigger names.
        /// <remarks>
        ///     Trigger name always should start with an annotation key, there is no other way to find
        ///     the trigger annotations while generating migrations besides to get all annotations with prefixes.
        /// </remarks>
        /// </summary>
        public static Func<TriggerTime, TriggerEvent, Type, string> GetTriggerName { get; set; } = GetTriggerNameInternal;

        private static string GetTriggerNameInternal(
            TriggerTime triggerTime,
            TriggerEvent triggerEvent,
            Type triggerEntityType)
        {
            return $"{triggerTime}_{triggerEvent}_{triggerEntityType.Name}".ToUpper();
        }
    }
}