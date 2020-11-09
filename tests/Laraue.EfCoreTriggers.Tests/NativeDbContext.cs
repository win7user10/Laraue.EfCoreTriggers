using Laraue.EfCoreTriggers.Extensions;
using Laraue.EfCoreTriggers.Tests.Entities;
using Microsoft.EntityFrameworkCore;

namespace Laraue.EfCoreTriggers.Tests
{
    public class NativeDbContext : DbContext
    {
        public DbSet<UserBalance> Balances { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<TransactionMirror> TransactionsMirror { get; set; }

        public DbSet<TestEntity> TestEntities { get; set; }

        public NativeDbContext(DbContextOptions<NativeDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .HasOne(x => x.User)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<TransactionMirror>()
                .HasOne(x => x.User)
                .WithMany(x => x.MirroredTransactions)
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
                            (insertedTransaction, oldUserBalance) => new UserBalance { Balance = oldUserBalance.Balance + insertedTransaction.Value }))
                    .Action(action => action
                        .Condition(insertedTransaction => !insertedTransaction.IsVeryfied)
                        .InsertIfNotExists(
                            balance => new { balance.UserId },
                            insertedTransaction => new UserBalance { UserId = insertedTransaction.UserId, Balance = 0 }))
                    .Action(action => action
                        .Insert(
                            insertedTransaction => new TransactionMirror
                            {
                                Id = insertedTransaction.Id,
                                UserId = insertedTransaction.UserId,
                                IsVeryfied = insertedTransaction.IsVeryfied,
                                Value = insertedTransaction.Value,
                                Description = insertedTransaction.Description.ToUpper() + "-COPY".ToLower(),
                            })));

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
                        .Update<TransactionMirror>(
                            (oldTransaction, updatedTransaction, mirroredTransaction) => mirroredTransaction.Id == updatedTransaction.Id,
                            (oldTransaction, updatedTransaction, mirroredTransaction) => new TransactionMirror
                            {
                                IsVeryfied = updatedTransaction.IsVeryfied,
                                Value = updatedTransaction.Value,
                            })));

            modelBuilder.Entity<Transaction>()
                .AfterDelete(trigger => trigger
                    .Action(action => action
                        .Condition(deletedTransaction => deletedTransaction.IsVeryfied)
                        .Update<UserBalance>(
                            (deletedTransaction, userBalances) => userBalances.UserId == deletedTransaction.UserId,
                            (deletedTransaction, oldBalance) => new UserBalance { Balance = oldBalance.Balance - deletedTransaction.Value }))
                    .Action(action => action
                        .Delete<TransactionMirror>((deletedTransaction, mirroredTransaction) => deletedTransaction.Id == mirroredTransaction.Id )));
        }
    }
}