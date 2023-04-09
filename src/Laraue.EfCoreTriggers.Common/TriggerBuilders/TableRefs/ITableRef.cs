namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs
{
    /// <inheritdoc />
    /// <typeparam name="TEntity">Type of the table that was triggered.</typeparam>
    public interface ITableRef<TEntity> : ITableRef
        where TEntity : class
    {
    }

    /// <summary>
    /// Contains references to the table row when trigger was fired.
    /// </summary>
    public interface ITableRef
    {
    }
}