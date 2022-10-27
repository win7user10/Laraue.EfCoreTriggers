using System;

namespace Laraue.EfCoreTriggers.Tests.Infrastructure;

public class BaseEntity
{
    public int Id { get; set; }
    public string StringField { get; set; }
    public char? CharValue { get; set; }
    public DateTimeOffset? DateTimeOffsetValue { get; set; }
}