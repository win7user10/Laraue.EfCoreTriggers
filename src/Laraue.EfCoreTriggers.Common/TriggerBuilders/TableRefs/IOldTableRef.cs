namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs
{
    /// <summary>
    /// Contains references to the table row before it was updated.
    /// </summary>
    public interface IOldTableRef<TEntity> : ITableRef<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Reference to the old entity in a trigger.
        /// </summary>
        public TEntity Old { get; }
    }
}