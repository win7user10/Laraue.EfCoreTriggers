namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;

/// <summary>
/// Contains reference to the old row in a trigger.
/// </summary>
public interface IOldTableRef<TEntity> : ITableRef<TEntity>
    where TEntity : class
{
    /// <summary>ы
    /// Reference to the old entity in a trigger.
    /// </summary>
    public TEntity Old { get; }
}