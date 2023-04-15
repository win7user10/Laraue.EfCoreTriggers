namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs
{
    /// <summary>
    /// Contains references to the table row before it was updated and after it was updated.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public sealed class OldAndNewTableRefs<TEntity> : INewTableRef<TEntity>, IOldTableRef<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Initializes a new instance of <see cref="OldAndNewTableRefs{TEntity}"/>.
        /// </summary>
        /// <param name="old"></param>
        /// <param name="new"></param>
        public OldAndNewTableRefs(TEntity old, TEntity @new)
        {
            Old = old;
            New = @new;
        }

        /// <inheritdoc />
        public TEntity New { get; }
    
        /// <inheritdoc />
        public TEntity Old { get; }
    }
}