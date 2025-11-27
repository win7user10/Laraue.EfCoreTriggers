using System;

namespace Laraue.Triggers.Core;

public interface ITriggerEntityType
{
    Type ClrType { get; }
    string? GetSchema();
}