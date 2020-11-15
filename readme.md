Entity Framework Core Triggers
--------------------

[![latest version](https://img.shields.io/nuget/v/Laraue.EfCoreTriggers)](https://www.nuget.org/packages/Laraue.EfCoreTriggers) [![downloads](https://img.shields.io/nuget/dt/Laraue.EfCoreTriggers)](https://www.nuget.org/packages/Laraue.EfCoreTriggers)

EfCoreTriggers is the library to write native SQL triggers using EFCore model builder. Triggers are automatically translating into sql and adding to migrations.

## Current available providers

- PostgreSQL
- SQL Server

### Configuring DB to use triggers

Call UseTriggers() to use the library.

```cs
var options = new DbContextOptionsBuilder<TestDbContext>()
    .UseNpgsql("User ID=test;Password=test;Host=localhost;Port=5432;Database=test;")
    .UseTriggers() // Initialize necessary dependencies
    .Options;

var dbContext = new TestDbContext(options);
```

After update Transaction entity, update records in the table with UserBalance entities.

```cs
modelBuilder.Entity<Transaction>()
    .AfterUpdate(trigger => trigger
        .Action(action => action
            .Condition((oldTransaction, newTransaction) => oldTransaction.IsVeryfied && newTransaction.IsVeryfied) // Executes only if condition met 
            .Update<UserBalance>(
                (oldTransaction, updatedTransaction, userBalances) => userBalances.UserId == oldTransaction.UserId, // Will be updated entities with matched condition
                (oldTransaction, updatedTransaction, oldBalance) => new UserBalance { Balance = oldBalance.Balance + updatedTransaction.Value - oldTransaction.Value }))); // New values for matched entities.
```

After Insert trigger entity, upsert record in the table with UserBalance entities.

```cs
modelBuilder.Entity<Transaction>()
    .AfterDelete(trigger => trigger
        .Action(action => action
            .Condition(deletedTransaction => deletedTransaction.IsVeryfied)
            .Upsert(
                balance => new { balance.UserId }, // If this field is matched, will be executed update operation else insert
                insertedTransaction => new UserBalance { UserId = insertedTransaction.UserId, Balance = insertedTransaction.Value }, // Insert, if value didn't exist
                (insertedTransaction, oldUserBalance) => new UserBalance { Balance = oldUserBalance.Balance + insertedTransaction.Value }))); // Update if value existed
```

More examples of using are available in Tests/NativeDbContext.cs.

#### All available triggers

||PostgreSql| SqlServer |
|Before Insert|+|-|
|After Insert|+|+|
|Instead Of Insert|+|+|
|Before Update|+|-|
|After Update|+|+|
|Instead Of Update|+|+|
|Before Delete|+|-|
|After Delete|+|+|
|Instead Of Delete|+|+|

#### Available actions after trigger has worked

- Insert
- InsertIfNotExists
- Update
- Upsert
- Delete