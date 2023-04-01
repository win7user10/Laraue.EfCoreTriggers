namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

public abstract class Refs<T>
{
}

public sealed class NewRef<T> : Refs<T>
{
    public NewRef(T @new)
    {
        New = @new;
    }

    public T New { get; }
}

public sealed class OldRef<T> : Refs<T>
{
    public OldRef(T old)
    {
        Old = old;
    }

    public T Old { get; }
}

public sealed class OldAndNewRef<T> : Refs<T>
{
    public OldAndNewRef(T old, T @new)
    {
        Old = old;
        New = @new;
    }

    public T Old { get; }
    public T New { get; }
}