namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;

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
    
    /// <summary>
    /// Gets the new entity from the current table refs.
    /// </summary>
    /// <param name="tableRef"></param>
    /// <returns></returns>
    public static implicit operator TEntity(NewTableRef<TEntity> tableRef)
    {
        return tableRef.New;
    }
}