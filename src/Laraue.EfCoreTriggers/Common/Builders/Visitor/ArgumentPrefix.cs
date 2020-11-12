namespace Laraue.EfCoreTriggers.Common.Builders.Visitor
{
    /// <summary>
    /// Arguments prefixes for using in SQL generating.
    /// </summary>
    public enum ArgumentPrefix
    {
        /// <summary>
        /// Entity without prefix.
        /// </summary>
        None,

        /// <summary>
        /// New entity prefix.Available on insert and update operations.
        /// </summary>
        New,

        /// <summary>
        /// Old entity prefix. Available on update and delete operations.
        /// </summary>
        Old,
    }
}
