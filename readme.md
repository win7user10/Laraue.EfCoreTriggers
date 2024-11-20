# Entity Framework Core Triggers

EfCoreTriggers is the library to write native SQL triggers using EFCore model builder. Triggers are automatically translating into sql and adding to migrations.

[![latest version](https://img.shields.io/nuget/v/Laraue.EfCoreTriggers.Common)](https://www.nuget.org/packages/Laraue.EfCoreTriggers.Common)
[![latest version](https://img.shields.io/nuget/dt/Laraue.EfCoreTriggers.Common)](https://www.nuget.org/packages/Laraue.EfCoreTriggers.Common)

#### Versions compatability
| Package version | .NET version     | EF Core version |
|-----------------|------------------|-----------------|
| 9.x.x           | NET 9.0          | 9               |
| 8.x.x           | NET 8.0          | 8               |
| 7.x.x           | NET 6.0          | 7               |
| 5.x.x           | NET standard 2.1 | 5               |

Install the provider package corresponding to your target database.

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
            .Condition(tableRefs => tableRefs.Old.IsVeryfied && tableRefs.New.IsVeryfied) // Executes only if condition met 
            .Update<UserBalance>(
                (tableRefs, userBalances) => userBalances.UserId == tableRefs.Old.UserId, // Will be updated entities with matched condition
                (tableRefs, oldBalance) => new UserBalance { Balance = oldBalance.Balance + tableRefs.New.Value - tableRefs.Old.Value }))); // New values for matched entities.
```

After insert Transaction entity, upsert record in the table with UserBalance entities.

```cs
modelBuilder.Entity<Transaction>()
    .AfterDelete(trigger => trigger
        .Action(action => action
            .Condition(tableRefs => tableRefs.Old.IsVeryfied)
            .Upsert(
                (tableRefs, balances) => tableRefs.Old.UserId == balances.UserId, // If this field will match more than 0 rows, will be executed update operation for these rows else insert
                tableRefs => new UserBalance { UserId = tableRefs.Old.UserId, Balance = tableRefs.Old.Value }, // Insert, if value didn't exist
                (tableRefs, oldUserBalance) => new UserBalance { Balance = oldUserBalance.Balance + tableRefs.Old.Value }))); // Update all matched values
```

After delete Transaction entity, execute raw SQL. Pass deleted entity fields as arguments. 

```cs
modelBuilder.Entity<Transaction>()
    .AfterDelete(trigger => trigger
        .Action(action => action
            .ExecuteRawSql("PERFORM recalc_balance({0}, {1})"), tableRefs => tableRefs.Old.UserId, tableRefs => tableRefs.Old.Amount)));
```

Also, different trigger functions can be used to generate the SQL.  
```
TriggerFunctions.GetTableName<Transaction>();
TriggerFunctions.GetColumnName<Transaction>(transaction => transaction.Value);
```

### All available triggers

| Trigger | PostgreSql | SQL Server | SQLite | MySQL |
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
- ExecuteRawSql

## Laraue.EfCoreTriggers.PostgreSql

[![latest version](https://img.shields.io/nuget/v/Laraue.EfCoreTriggers.PostgreSql)](https://www.nuget.org/packages/Laraue.EfCoreTriggers.PostgreSql)
[![latest version](https://img.shields.io/nuget/dt/Laraue.EfCoreTriggers.PostgreSql)](https://www.nuget.org/packages/Laraue.EfCoreTriggers.PostgreSql)

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
[![latest version](https://img.shields.io/nuget/dt/Laraue.EfCoreTriggers.MySql)](https://www.nuget.org/packages/Laraue.EfCoreTriggers.MySql)

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
[![latest version](https://img.shields.io/nuget/dt/Laraue.EfCoreTriggers.SqlServer)](https://www.nuget.org/packages/Laraue.EfCoreTriggers.SqlServer)

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
[![latest version](https://img.shields.io/nuget/dt/Laraue.EfCoreTriggers.SqlLite)](https://www.nuget.org/packages/Laraue.EfCoreTriggers.SqlLite)

### Basic usage

```cs
var options = new DbContextOptionsBuilder<TestDbContext>()
    .UseSqlite("Filename=D://test.db")
    .UseSqlLiteTriggers()
    .Options;

var dbContext = new TestDbContext(options);
```

## Customization

Any service using for generation SQL can be replaced.

```cs
private class CustomDbSchemaRetriever : EfCoreDbSchemaRetriever
{
    public CustomDbSchemaRetriever(IModel model) : base(model)
    {
    }

    protected override string GetColumnName(Type type, MemberInfo memberInfo)
    {
        // Change strategy of naming some column
        return 'c_' + base.GetColumnName(type, memberInfo);
    }
}
```

Adding this service to the container

```cs
var options = new DbContextOptionsBuilder<TestDbContext>()
    .UseNpgsql("User ID=test;Password=test;Host=localhost;Port=5432;Database=test;")
    .UsePostgreSqlTriggers(services => services.AddSingleton<IDbSchemaRetriever, CustomDbSchemaRetriever>)
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
        throw new InvalidOperationException();
    }
} 
```

Now a custom converter should be written to translate this function into SQL

```cs
public abstract class StringExtensionsLikeConverter : MethodCallConverter
{
    public override bool IsApplicable(MethodCallExpression expression)
    {
        return expression.Method.ReflectedType == typeof(StringExtensions) && MethodName == nameof(StringExtensions.Like);
    }
    
    public override SqlBuilder BuildSql(BaseExpressionProvider provider, MethodCallExpression expression)
    {
        // Generate SQL for arguments, they can be SQL expressions
        var argumentSql = provider.GetMethodCallArgumentsSql(expression)[0];

        // Generate SQL for this context, it also can be a SQL expression
        var sqlBuilder = provider.GetExpressionSql(expression.Object);
        
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
    .UseSqlLiteTriggers(services => services.AddMethodCallConverter(converter))
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

### Trigger prefix customization

You can change the standard library prefix for trigger using the next static variable
```cs
Laraue.EfCoreTriggers.Common.Constants.AnnotationKey = "MY_PREFIX"
```

### Generic triggers

Define the generic trigger class inherits `GenericTrigger<BaseTriggerClass>`
```cs
// Trigger for all classes inherits class Notification
private sealed class NotificationsTriggers : GenericTrigger<Notification>
{
    public override void ApplyTrigger<TImplEntity>(EntityTypeBuilder<TImplEntity> modelBuilder)
    {
        modelBuilder
            .AfterInsert(x => x
                .Action(y => y
                    .Insert<NotificationLog>(inserted => new NotificationLog
                    {
                        Text = inserted.New.Text,
                        NotificationType = typeof(TImplEntity).ToString(),
                        OriginalId = inserted.New.Id
                    })));
    }
}
```

Registering generic trigger
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.AddGenericTrigger(new NotificationsTriggers());
}
```