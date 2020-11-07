using System;
using System.Collections.Generic;

namespace Laraue.EfCoreTriggers.Tests.Entities
{
    public class User
    {
        public Guid UserId { get; set; }

        public UserBalance Balance { get; set; }

        public IEnumerable<Transaction> Transactions { get; set; }

        public IEnumerable<TransactionMirror> MirroredTransactions { get; set; }
    }
}
