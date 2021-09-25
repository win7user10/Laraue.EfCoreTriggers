using System;

namespace Laraue.EfCoreTriggers.Tests.Infrastructure
{
    public class DestinationEntity
    {
        public int Id { get; set; }
        public string StringField { get; set; }
        public EnumValue? EnumValue { get; set; }
        public decimal? DecimalValue { get; set; }
        public double? DoubleValue { get; set; }
        public int? IntValue { get; set; }
        public bool? BooleanValue { get; set; }
        public Guid? GuidValue { get; set; }

        public int? UniqueIdentifier { get; set; }
    }
}