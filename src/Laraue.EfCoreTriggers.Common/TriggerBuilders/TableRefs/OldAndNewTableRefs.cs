using System;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;

/// <summary>
/// 
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

    /// <summary>
    /// Converts current class to the tuple.
    /// </summary>
    /// <param name="tableRefs"></param>
    /// <returns></returns>
    public static implicit operator Tuple<TEntity, TEntity>(OldAndNewTableRefs<TEntity> tableRefs)
    {
        return Tuple.Create(tableRefs.Old, tableRefs.New);
    }
}