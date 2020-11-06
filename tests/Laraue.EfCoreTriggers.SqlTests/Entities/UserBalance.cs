using System;

namespace Laraue.EfCoreTriggers.SqlTests.Entities
{
    public class UserBalance
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public decimal Balance { get; set; }

        public User User { get; set; }
    }
}
