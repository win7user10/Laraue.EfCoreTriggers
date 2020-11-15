namespace Laraue.EfCoreTriggers.Common.Builders.Providers
{
    /// <summary>
    /// Arguments types for using in SQL generating.
    /// </summary>
    public enum ArgumentType
    {
        /// <summary>
        /// For entity should be used annotation ColumnName
        /// </summary>
        None,

        /// <summary>
        /// For entity should be used annotation NewPrefix.ColumnName
        /// </summary>
        New,

        /// <summary>
        /// For entity should be used annotation OldPrefix.ColumnName
        /// </summary>
        Old,

        /// <summary>
        /// For entity should be used annotation TableName.ColumnName
        /// </summary>
        Default,
    }
}
