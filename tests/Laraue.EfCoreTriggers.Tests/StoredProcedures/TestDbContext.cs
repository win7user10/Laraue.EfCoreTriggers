using Laraue.EfCoreTriggers.CSharpBuilder;
using Laraue.EfCoreTriggers.Tests.StoredProcedures.Entities;
using Microsoft.EntityFrameworkCore;

namespace Laraue.EfCoreTriggers.Tests.StoredProcedures
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
                        .UpdateAnotherEntity<UserBalance>(
                            (oldTransaction, newTransaction, userBalances) => userBalances.UserId == oldTransaction.UserId,
                            (oldTransaction, newTransaction, oldBalance) => new UserBalance { Balance = oldBalance.Balance + newTransaction.Value - oldTransaction.Value })));

            modelBuilder.Entity<Transaction>()
                .AfterDelete(trigger => trigger
                    .Action(action => action
                        .Condition(deletedTransaction => deletedTransaction.IsVeryfied)
                        .UpdateAnotherEntity<UserBalance>(
                            (deletedTransaction, userBalances) => userBalances.UserId == deletedTransaction.UserId,
                            (deletedTransaction, oldUser) => new UserBalance { Balance = oldUser.Balance - deletedTransaction.Value })));

            modelBuilder.Entity<Transaction>()
                .AfterInsert(trigger => trigger
                    .Action(action => action
                        .Condition(insertedTransaction => insertedTransaction.IsVeryfied)
                        .UpdateAnotherEntity<UserBalance>(
                            (insertedTransaction, userBalances) => userBalances.UserId == insertedTransaction.UserId,
                            (insertedTransaction, oldUser) => new UserBalance { Balance = oldUser.Balance + insertedTransaction.Value })));
        }
    }

    
}
