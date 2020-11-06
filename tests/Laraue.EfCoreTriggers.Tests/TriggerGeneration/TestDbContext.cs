using Laraue.EfCoreTriggers.CSharpBuilder;
using Laraue.EfCoreTriggers.Tests.Entities;
using Microsoft.EntityFrameworkCore;

namespace Laraue.EfCoreTriggers.Tests.TriggerGeneration
{
    public class TestDbContext : DbContext
    {
        public DbSet<UserBalance> Balances { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .AfterUpdate(trigger => trigger
                    .Action(action => action
                        .Condition((oldTransaction, newTransaction) => oldTransaction.IsVeryfied && newTransaction.IsVeryfied)
                        .Upsert(
                            balance => new { balance.UserId },
                            (oldTransaction, updatedTransaction) => new UserBalance { Balance = updatedTransaction.Value - oldTransaction.Value },
                            (oldTransaction, updatedTransaction, oldUserBalance) => new UserBalance { Balance = updatedTransaction.Value - oldTransaction.Value + oldUserBalance.Balance })
                        .Update<UserBalance>(
                            (oldTransaction, newTransaction, userBalances) => userBalances.UserId == oldTransaction.UserId,
                            (oldTransaction, newTransaction, oldBalance) => new UserBalance { Balance = oldBalance.Balance + newTransaction.Value - oldTransaction.Value })));

            modelBuilder.Entity<Transaction>()
                .AfterDelete(trigger => trigger
                    .Action(action => action
                        .Condition(deletedTransaction => deletedTransaction.IsVeryfied)
                        .Upsert(
                            balance => new { balance.UserId },
                            deletedTransaction => new UserBalance { Balance = -deletedTransaction.Value },
                            (deletedTransaction, oldUserBalance) => new UserBalance { Balance = oldUserBalance.Balance - deletedTransaction.Value })
                        .Update<UserBalance>(
                            (deletedTransaction, userBalances) => userBalances.UserId == deletedTransaction.UserId,
                            (deletedTransaction, oldUser) => new UserBalance { Balance = oldUser.Balance - deletedTransaction.Value })));

            modelBuilder.Entity<Transaction>()
                .AfterInsert(trigger => trigger
                    .Action(action => action
                        .Condition(insertedTransaction => insertedTransaction.IsVeryfied)
                        .Upsert(
                            balance => new { balance.UserId },
                            insertedTransaction => new UserBalance { Balance = insertedTransaction.Value },
                            (insertedTransaction, oldUserBalance) => new UserBalance { Balance = oldUserBalance.Balance + insertedTransaction.Value })
                        .Update<UserBalance>(
                            (insertedTransaction, userBalances) => userBalances.UserId == insertedTransaction.UserId,
                            (insertedTransaction, oldUser) => new UserBalance { Balance = oldUser.Balance + insertedTransaction.Value })));
        }
    }

    
}
