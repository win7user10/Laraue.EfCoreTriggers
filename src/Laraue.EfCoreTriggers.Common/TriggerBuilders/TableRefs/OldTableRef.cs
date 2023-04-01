namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;

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
    
    /// <summary>
    /// Gets the old entity from the current table refs.
    /// </summary>
    /// <param name="tableRef"></param>
    /// <returns></returns>
    public static implicit operator TEntity(OldTableRef<TEntity> tableRef)
    {
        return tableRef.Old;
    }
}