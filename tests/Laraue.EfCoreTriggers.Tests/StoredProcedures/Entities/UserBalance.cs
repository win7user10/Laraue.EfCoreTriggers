using System;

namespace Laraue.EfCoreTriggers.Tests.StoredProcedures.Entities
{
    public class UserBalance
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public decimal Balance { get; set; }
    }
}
