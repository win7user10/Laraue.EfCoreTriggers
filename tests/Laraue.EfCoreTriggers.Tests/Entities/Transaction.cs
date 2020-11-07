﻿using System;

namespace Laraue.EfCoreTriggers.Tests.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }

        public decimal Value { get; set; }

        public bool IsVeryfied { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}
