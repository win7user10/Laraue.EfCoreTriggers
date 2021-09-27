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
            .Condition((transactionBeforeUpdate, transactionAfterUpdate) => transactionBeforeUpdate.IsVeryfied && transactionAfterUpdate.IsVeryfied) // Executes only if condition met 
            .Update<UserBalance>(
                (transactionBeforeUpdate, transactionAfterUpdate, userBalances) => userBalances.UserId == oldTransaction.UserId, // Will be updated entities with matched condition
                (oldTransaction, updatedTransaction, oldBalance) => new UserBalance { Balance = oldBalance.Balance + updatedTransaction.Value - oldTransaction.Value }))); // New values for matched entities.
```

After Insert trigger entity, upsert record in the table with UserBalance entities.

```cs
modelBuilder.Entity<Transaction>()
    .AfterDelete(trigger => trigger
        .Action(action => action
            .Condition(deletedTransaction => deletedTransaction.IsVeryfied)
            .Upsert(
                deletedTransaction => new UserBalance { UserId = deletedTransaction.UserId }, // If this field will match more than 0 rows, will be executed update operation for these rows else insert
                deletedTransaction => new UserBalance { UserId = deletedTransaction.UserId, Balance = deletedTransaction.Value }, // Insert, if value didn't exist
                (deletedTransaction, oldUserBalance) => new UserBalance { Balance = oldUserBalance.Balance + deletedTransaction.Value }))); // Update all matched values
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

### Customization

Using custom provider to extend additional functionality

```cs
private class MyCustomSqlProvider : PostgreSqlProvider // Or another used provider
{
    /// Provider will be created via reflection, so constructor only with this argument is allowed 
    public MySqlProvider(IModel model) : base(model)
    {
    }

    protected override string GetColumnName(MemberInfo memberInfo)
    {
        // Change strategy of naming some column
        return 'c_' + base.GetColumnName(memberInfo);
    }
}
```

Adding this provider to a container

```cs
var options = new DbContextOptionsBuilder<TestDbContext>()
    .UseNpgsql("User ID=test;Password=test;Host=localhost;Port=5432;Database=test;")
    .UseTriggers<MyCustomSqlProvider>()
    .Options;

var dbContext = new TestDbContext(options);
```

### Adding translation of some custom function into sql code

To do this thing a custom function converter should be added to a provider

Let's image that we have an extension like

```cs
public static class StringExtensions
{
    public static bool Like(this string str, string pattern)
    {
        // Some code
    }
} 
```

Now a custom converter should be written to translate this function into SQL

```cs
public abstract class StringExtensionsLikeConverter : MethodCallConverter
{
    public override bool IsApplicable(MethodCallExpression expression)
    {
        return expression.Method.ReflectedType == typeof(SomeFunctions) && MethodName == nameof(CustomFunctions.Like);
    }
    
    public override SqlBuilder BuildSql(BaseExpressionProvider provider, MethodCallExpression expression, Dictionary<string, ArgumentType> argumentTypes)
    {
        // Generate SQL for arguments, they can be SQL expressions
        var argumentSql = provider.GetMethodCallArgumentsSql(expression, argumentTypes)[0];

        // Generate SQL for this context, it also can be a SQL expression
        var sqlBuilder = provider.GetExpressionSql(expression.Object, argumentTypes);
        
        // Combine SQL for object and SQL for arguments
        // Output will be like "thisValueSql LIKE 'passedArgumentValueSql'"
        return new(sqlBuilder.AffectedColumns, $"{sqlBuilder} LIKE {argumentSql}");
    }
}
```

All custom converters should be added while setup a database

```cs
var options = new DbContextOptionsBuilder<TestDbContext>()
    .UseSqlite("Filename=D://test.db")
    .UseSqlLiteTriggers(converters => converters.ExpressionCallConverters.Push(converter))
    .Options;

var dbContext = new TestDbContext(options);
```

Now this function can be used in a trigger and it will be translated into SQL

```cs
modelBuilder.Entity<Transaction>()
    .AfterDelete(trigger => trigger
        .Action(action => action
            .Condition(oldTransaction => oldTransaction.Description.Like('%payment%'))
            
```