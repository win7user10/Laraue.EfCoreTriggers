using System;
using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.OnInsert;
using Laraue.EfCoreTriggers.MySql.Extensions;
using Laraue.EfCoreTriggers.Tests.Entities;
using Laraue.EfCoreTriggers.Tests.Enums;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.Tests.Tests
{
    [UnitTest]
    public class InheritanceTests
    {
        private readonly ITriggerActionVisitorFactory _provider;

        private sealed class DatabaseContext : DbContext
        {
            public DatabaseContext()
                : base(new DbContextOptionsBuilder<DatabaseContext>()
                    .UseSqlite("Data Source=:memory:").Options)
            {
            }
            
            public EmailNotification EmailNotifications { get; set; }
        }

        private abstract class Notification
        {
            public int Id { get; set; }
            public string Text { get; set; }
        }

        private sealed class EmailNotification : Notification
        {
        }
        
        public InheritanceTests()
        {
            _provider = Helper.GetTriggerActionFactory(
                new DatabaseContext().Model,
                collection => collection.AddMySqlServices());
        }
        
        [Fact]
        public virtual void Entity_NotExistsInDbContext_ShouldThrowException()
        {
            var trigger = new OnInsertTrigger<Notification>(TriggerTime.After, typeof(EmailNotification));

            trigger.Action(actions => actions.ExecuteRawSql(
                "select 1 from {0}",
                notification => notification));

            var sql = _provider.Visit(trigger.Actions[0], new VisitedMembers());
            
            Assert.Equal("select 1 from notifications", sql);
        }
    }
}