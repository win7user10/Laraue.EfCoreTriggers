﻿using System;

namespace Laraue.EfCoreTriggers.Tests.Entities
{
    public class UserBalance
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public decimal Balance { get; set; }

        public User User { get; set; }
    }
}
