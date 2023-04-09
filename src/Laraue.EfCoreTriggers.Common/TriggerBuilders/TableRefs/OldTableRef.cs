namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs
{
    /// <inheritdoc />
    public sealed class OldTableRef<TEntity> : IOldTableRef<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Initializes a new instance of <see cref="OldTableRef{TEntity}"/>.
        /// </summary>
        /// <param name="old"></param>
        public OldTableRef(TEntity old)
        {
            Old = old;
        }

        /// <inheritdoc />
        public TEntity Old { get; }
    }
}