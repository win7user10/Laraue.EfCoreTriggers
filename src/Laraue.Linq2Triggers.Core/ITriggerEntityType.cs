using System;

namespace Laraue.Linq2Triggers.Core;

public interface ITriggerEntityType
{
    Type ClrType { get; }
    string? GetSchema();
}