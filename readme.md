EfCoreTriggers is a library to write triggers for EFCore using OOP syntax. Triggers are translating into sql and adding to migrations.

## Available providers

- PostgreSQL

### Configuring DB to use triggers

```cs
var options = new DbContextOptionsBuilder<TestDbContext>()
  .UseNpgsql("User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=tests;")
  .UseTriggers() // Initialize necessary dependencies
  .Options;
var dbContext = new TestDbContext(options);
```