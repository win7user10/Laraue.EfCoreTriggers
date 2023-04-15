namespace Laraue.EfCoreTriggers.Common
{
    /// <summary>
    /// Library constants
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// All triggers names starts with this key. Acronym from laraue core trigger.
        /// Note: if triggers already created with one prefix, it's changing will be
        /// a problem, because this prefix is using to find trigger annotations
        /// while migrations generating.
        /// The best way will be to generate a new migrations, manually fix they names
        /// to start from the new <see cref="AnnotationKey"/>, only then change the value.
        /// </summary>
        public static string AnnotationKey { get; set; } = "LC_TRIGGER";
    }
}