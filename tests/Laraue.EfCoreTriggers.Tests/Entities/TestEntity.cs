using System;

namespace Laraue.EfCoreTriggers.Tests.Entities
{
    public class TestEntity
    {
        public int Id { get; set; }

        public decimal DecimalValue { get; set; }

        public double DoubleValue { get; set; }

        public int IntValue { get; set; }

        public Guid GuidValue { get; set; }

        public bool BooleanValue { get; set; }

        public char CharValue { get; set; }
    }
}