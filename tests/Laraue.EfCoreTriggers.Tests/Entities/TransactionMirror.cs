using System;

namespace Laraue.EfCoreTriggers.Tests.Entities
{
    public class TransactionMirror
    {
        public Guid Id { get; set; }

        public decimal Value { get; set; }

        public double DoubleValue { get; set; }

        public bool IsVeryfied { get; set; }

        public string Description { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}