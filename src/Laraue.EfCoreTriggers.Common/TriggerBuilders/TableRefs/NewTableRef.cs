namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs
{
    /// <inheritdoc />
    public sealed class NewTableRef<TEntity> : INewTableRef<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NewTableRef{TEntity}"/>.
        /// </summary>
        /// <param name="new"></param>
        public NewTableRef(TEntity @new)
        {
            New = @new;
        }

        /// <inheritdoc />
        public TEntity New { get; }
    }
}