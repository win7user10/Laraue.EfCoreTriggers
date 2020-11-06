using System;
using System.Collections.Generic;

namespace Laraue.EfCoreTriggers.SqlTests.Entities
{
    public class User
    {
        public Guid UserId { get; set; }

        public UserBalance Balance { get; set; }

        public IEnumerable<Transaction> Transactions { get; set; }
    }
}
