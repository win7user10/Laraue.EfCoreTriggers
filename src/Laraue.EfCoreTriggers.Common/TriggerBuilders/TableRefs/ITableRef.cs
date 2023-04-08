namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;

/// <summary>
/// Represents the class with references to triggered table rows. 
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface ITableRef<TEntity> : ITableRef
    where TEntity : class
{
}

public interface ITableRef
{
}