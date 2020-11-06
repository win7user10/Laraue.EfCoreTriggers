using Laraue.EfCoreTriggers.CSharpBuilder;
using Laraue.EfCoreTriggers.SqlTests.Entities;
using Microsoft.EntityFrameworkCore;

namespace Laraue.EfCoreTriggers.SqlTests
{
    public class NativeDbContext : DbContext
    {
        public DbSet<UserBalance> Balances { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<User> Users { get; set; }

        public NativeDbContext(DbContextOptions<NativeDbContext> options) : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .HasOne(x => x.User)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<User>()
                .HasOne(x => x.Balance);

            modelBuilder.Entity<UserBalance>()
                .HasOne(x => x.User);

            modelBuilder.Entity<Transaction>()
                .AfterInsert(trigger => trigger
                    .Action(action => action
                        .Condition(insertedTransaction => insertedTransaction.IsVeryfied)
                        .Upsert(
                            balance => new { balance.UserId },
                            insertedTransaction => new UserBalance { UserId = insertedTransaction.UserId, Balance = insertedTransaction.Value },
                            (insertedTransaction, oldUserBalance) => new UserBalance { Balance = oldUserBalance.Balance + insertedTransaction.Value })));

            modelBuilder.Entity<Transaction>()
                .AfterUpdate(trigger => trigger
                    .Action(action => action
                        .Condition((oldTransaction, newTransaction) => oldTransaction.IsVeryfied && newTransaction.IsVeryfied)
                        .Update<UserBalance>(
                            (oldTransaction, updatedTransaction, userBalances) => userBalances.UserId == oldTransaction.UserId,
                            (oldTransaction, updatedTransaction, oldBalance) => new UserBalance { Balance = oldBalance.Balance + updatedTransaction.Value - oldTransaction.Value }))
                    .Action(action => action
                        .Condition((oldTransaction, newTransaction) => !oldTransaction.IsVeryfied && newTransaction.IsVeryfied)
                        .Update<UserBalance>(
                            (oldTransaction, updatedTransaction, userBalances) => userBalances.UserId == oldTransaction.UserId,
                            (oldTransaction, updatedTransaction, oldBalance) => new UserBalance { Balance = oldBalance.Balance + updatedTransaction.Value }))
                    .Action(action => action
                        .Condition((oldTransaction, newTransaction) => oldTransaction.IsVeryfied && !newTransaction.IsVeryfied)
                        .Update<UserBalance>(
                            (oldTransaction, updatedTransaction, userBalances) => userBalances.UserId == oldTransaction.UserId,
                            (oldTransaction, updatedTransaction, oldBalance) => new UserBalance { Balance = oldBalance.Balance - oldTransaction.Value })));

            modelBuilder.Entity<Transaction>()
                .AfterDelete(trigger => trigger
                    .Action(action => action
                        .Condition(deletedTransaction => deletedTransaction.IsVeryfied)
                        .Update<UserBalance>(
                            (deletedTransaction, userBalances) => userBalances.UserId == deletedTransaction.UserId,
                            (deletedTransaction, oldBalance) => new UserBalance { Balance = oldBalance.Balance - deletedTransaction.Value })));
        }
    }

    
}
