using Laraue.EfCoreTriggers.Tests.Entities;
using LinqToDB;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Laraue.EfCoreTriggers.Tests
{
    public abstract class BaseNativeTests
    {
        protected readonly NativeDbContext DbContext;

        private readonly Guid UserId = Guid.NewGuid();

        public BaseNativeTests(NativeDbContext dbContext)
        {
            DbContext = dbContext;
            LinqToDBForEFTools.Initialize();
            InitializeDbContext();
            ClearTables();
            CreateUser();
        }

        protected virtual void InitializeDbContext()
        {
        }

        protected void ClearTables() => DbContext.Users.Delete(_ => true);

        protected void CreateUser()
        {
            DbContext.Users.Add(new User { UserId = UserId });
            DbContext.SaveChanges();
        }

        protected async Task<Guid> AddTransactionAsync(bool isVerifyed, decimal value)
        {
            var transaction = new Transaction
            {
                IsVeryfied = isVerifyed,
                UserId = UserId,
                Value = value,
                Description = "test",
            };

            DbContext.Transactions.Add(transaction);
            await DbContext.SaveChangesAsync();
            return transaction.Id;
        }

        protected async Task<bool> UpdateTransactionAsync(Guid transactionId, bool isVerifyed, decimal value)
        {
            return await DbContext.Transactions
                .Where(x => x.Id == transactionId)
                .UpdateAsync(old => new Transaction
                {
                    IsVeryfied = isVerifyed,
                    Value = value,
                }) > 0;
        }

        protected async Task<bool> DeleteTransactionAsync(Guid transactionId)
        {
            return await DbContext.Transactions
                .Where(x => x.Id == transactionId)
                .DeleteAsync() > 0;
        }

        protected async Task VerifyMirroredTransactionAsync(Guid transactionId, bool isVerifyed, decimal value, string description = "TEST-copy")
        {
            var transaction = await DbContext.TransactionsMirror.Where(x => x.Id == transactionId)
                .FirstOrDefaultAsyncLinqToDB();
            Assert.NotNull(transaction);
            Assert.Equal(isVerifyed, transaction.IsVeryfied);
            Assert.Equal(value, transaction.Value);
            Assert.Equal(description, transaction.Description);
        }

        [Theory]
        [InlineData(true, 50)]
        [InlineData(false, 0)]
        public async Task InsertTransactionShouldInsertBalance(bool isVeryfied, decimal exceptedBalance)
        {
            var transactionId = await AddTransactionAsync(isVeryfied, 50);
            var balance = Assert.Single(DbContext.Balances);
            Assert.Equal(UserId, balance.UserId);
            Assert.Equal(exceptedBalance, balance.Balance);
            await VerifyMirroredTransactionAsync(transactionId, isVeryfied, 50);
        }

        [Theory]
        [InlineData(true, true, 70)]
        [InlineData(true, false, 50)]
        [InlineData(false, true, 70)]
        public async Task UpdateTransactionShouldUpdateBalance(bool isOldTransactionVeryfied, bool isNewTransactionVerifyed, decimal exceptedBalance)
        {
            var transactionId = await AddTransactionAsync(isOldTransactionVeryfied, 50);
            await UpdateTransactionAsync(transactionId, isNewTransactionVerifyed, 70);
            var balance = Assert.Single(DbContext.Balances);
            Assert.Equal(exceptedBalance, balance.Balance);
            await VerifyMirroredTransactionAsync(transactionId, isNewTransactionVerifyed, 70);
        }

        [Theory]
        [InlineData(true, 50)]
        [InlineData(false, 0)]
        public async Task DeleteTransactionShouldUpdateBalance(bool isVeryfied, decimal intermediateBalance)
        {
            var transactionId = await AddTransactionAsync(isVeryfied, 50);

            var balance = Assert.Single(DbContext.Balances.AsNoTracking());
            Assert.Equal(intermediateBalance, balance.Balance);
            await VerifyMirroredTransactionAsync(transactionId, isVeryfied, 50);

            await DeleteTransactionAsync(transactionId);

            balance = Assert.Single(DbContext.Balances.AsNoTracking());
            Assert.Equal(0, balance.Balance);

            Assert.Null(await DbContext.TransactionsMirror.Where(x => x.Id == transactionId).FirstOrDefaultAsyncLinqToDB());
        }
    }
}
