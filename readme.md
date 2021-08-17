# Entity Framework Core Triggers

EfCoreTriggers is the library to write native SQL triggers using EFCore model builder. Triggers are automatically translating into sql and adding to migrations.

[![latest version](https://img.shields.io/nuget/v/Laraue.EfCoreTriggers.Common)](https://www.nuget.org/packages/Laraue.EfCoreTriggers.Common)

### Installation
EfCoreTriggers common package is available on [NuGet](https://www.nuget.org/packages/Laraue.EfCoreTriggers.Common). Install the provider package corresponding to your target database. See the list of providers in the docs for additional databases.

### Configuring DB to use triggers

```sh
dotnet add package Laraue.EfCoreTriggers.PostgreSql
dotnet add package Laraue.EfCoreTriggers.MySql
dotnet add package Laraue.EfCoreTriggers.SqlServer
dotnet add package Laraue.EfCoreTriggers.SqlLite
```

### Basic usage

The library has extensions for EntityBuilder to configure DbContext. 

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

### All available triggers

| Trigger | PostgreSql | SQL Server | SQLite | MySQL
| --- | --- | --- | --- | --- |
| Before Insert | + | - | + | + |
| After Insert | + | + | + | + |
| Instead Of Insert | + | + | + | - |
| Before Update | + | - | + | + |
| After Update | + | + | + | + |
| Instead Of Update | + | + | + | - |
| Before Delete | + | - | + | + |
| After Delete | + | + | + | + |
| Instead Of Delete | + | + | + | - |

### Available actions after trigger has worked

- Insert
- InsertIfNotExists
- Update
- Upsert
- Delete

## Laraue.EfCoreTriggers.PostgreSql

[![latest version](https://img.shields.io/nuget/v/Laraue.EfCoreTriggers.PostgreSql)](https://www.nuget.org/packages/Laraue.EfCoreTriggers.PostgreSql)

### Basic usage

```cs
var options = new DbContextOptionsBuilder<TestDbContext>()
    .UseNpgsql("User ID=test;Password=test;Host=localhost;Port=5432;Database=test;")
    .UsePostgreSqlTriggers()
    .Options;

var dbContext = new TestDbContext(options);
```

## Laraue.EfCoreTriggers.MySql

[![latest version](https://img.shields.io/nuget/v/Laraue.EfCoreTriggers.MySql)](https://www.nuget.org/packages/Laraue.EfCoreTriggers.MySql)

### Basic usage

```cs
var options = new DbContextOptionsBuilder<TestDbContext>()
    .UseMySql("server=localhost;user=test;password=test;database=test;", new MySqlServerVersion(new Version(8, 0, 22))))
    .UseMySqlTriggers()
    .Options;

var dbContext = new TestDbContext(options);
```

## Laraue.EfCoreTriggers.SqlServer

[![latest version](https://img.shields.io/nuget/v/Laraue.EfCoreTriggers.SqlServer)](https://www.nuget.org/packages/Laraue.EfCoreTriggers.SqlServer)

### Basic usage

```cs
var options = new DbContextOptionsBuilder<TestDbContext>()
    .UseSqlServer("Data Source=(LocalDb)\\v15.0;Database=test;Integrated Security=SSPI;")
    .UseSqlServerTriggers()
    .Options;

var dbContext = new TestDbContext(options);
```
## Laraue.EfCoreTriggers.SqlLite

[![latest version](https://img.shields.io/nuget/v/Laraue.EfCoreTriggers.SqlLite)](https://www.nuget.org/packages/Laraue.EfCoreTriggers.SqlLite)

### Basic usage

```cs
var options = new DbContextOptionsBuilder<TestDbContext>()
    .UseSqlite("Filename=D://test.db")
    .UseSqlLiteTriggers()
    .Options;

var dbContext = new TestDbContext(options);
```