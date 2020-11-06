using Laraue.EfCoreTriggers.SqlTests;
using Laraue.EfCoreTriggers.SqlTests.Entities;
using LinqToDB;
using LinqToDB.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Laraue.EfCoreTriggers.Tests.NativeTests
{
    public abstract class BaseNativeTests
    {
        protected readonly NativeDbContext DbContext;

        private readonly Guid UserId = Guid.NewGuid();
        private const decimal DefaultTransactionValue = 50;

        public BaseNativeTests(NativeDbContext dbContext)
        {
            DbContext = dbContext;

            LinqToDBForEFTools.Initialize();

            ClearTables();

            CreateUser();
        }

        protected void ClearTables() => DbContext.Users.Delete(_ => true);

        protected void CreateUser()
        {
            DbContext.Users.Add(new User { UserId = UserId });
            DbContext.SaveChanges();
        }

        protected async Task<Guid> AddTransactionAsync(bool isVerifyed = true, decimal value = DefaultTransactionValue)
        {
            var transaction = new Transaction
            {
                IsVeryfied = isVerifyed,
                UserId = UserId,
                Value = value
            };
            DbContext.Transactions.Add(transaction);
            await DbContext.SaveChangesAsync();
            return transaction.Id;
        }

        [Fact]
        public async Task InsertBalanceAfterInsertTransaction()
        {
            await AddTransactionAsync();
            var balance = Assert.Single(DbContext.Balances);
            Assert.Equal(UserId, balance.UserId);
            Assert.Equal(DefaultTransactionValue, balance.Balance);
        }
    }
}
