namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;

/// <summary>
/// Contains reference to the new row in a trigger.
/// </summary>
public interface INewTableRef<TEntity> : ITableRef<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Reference to the new entity in a trigger.
    /// </summary>
    public TEntity New { get; }
}