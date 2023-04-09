namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs
{
    /// <summary>
    /// Contains references to the table row after it was updated.
    /// </summary>
    public interface INewTableRef<TEntity> : ITableRef<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Reference to the new entity in a trigger.
        /// </summary>
        public TEntity New { get; }
    }
}