using System;
using System.Collections.Generic;
using System.Linq;
using Laraue.EfCoreTriggers.Common.Functions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;
using Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors;
using Laraue.EfCoreTriggers.MySql.Extensions;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Xunit;
using Xunit.Categories;
using ITrigger = Laraue.EfCoreTriggers.Common.TriggerBuilders.Abstractions.ITrigger;

namespace Laraue.EfCoreTriggers.Tests.Tests
{
    [UnitTest]
    public class WorkaroundTests
    {
        private readonly ITriggerActionVisitorFactory _provider;
        private readonly IModel _model;

        private sealed class DatabaseContext : DbContext
        {
            public DatabaseContext()
                : base(new DbContextOptionsBuilder<DatabaseContext>()
                    .UseSqlite("Data Source=:memory:").Options)
            {
            }
            
            public DbSet<EmailNotification> EmailNotifications { get; set; }
            public DbSet<TelegramNotification> TelegramNotifications { get; set; }
        }

        private abstract class Notification
        {
            public int Id { get; set; }
            public string Text { get; set; }
        }

        private sealed class EmailNotification : Notification
        {
        }
        
        private sealed class TelegramNotification : Notification
        {
        }
        
        public WorkaroundTests()
        {
            _model = new DatabaseContext().Model;
            
            _provider = Helper.GetTriggerActionFactory(
                _model,
                collection => collection.AddMySqlServices());
        }
        
        [Fact]
        public virtual void Library_ShouldHaveOpportunityToRegisterNonGenericTriggers()
        {
            var types = _model.GetEntityTypes()
                .Where(t => t.ClrType.IsAssignableTo(typeof(Notification)));

            var sqlQueries = new List<string>();
            
            foreach (var type in types)
            {
                var triggerType = typeof(Trigger<,>).MakeGenericType(
                    type.ClrType,
                    typeof(NewTableRef<>).MakeGenericType(type.ClrType));
                var trigger = (ITrigger) Activator.CreateInstance(triggerType, TriggerEvent.Delete, TriggerTime.After)!;
                AddTriggerAction((dynamic) trigger);

                var sql = _provider.Visit(trigger.Actions[0].ActionExpressions.First(), new VisitedMembers());
                
                sqlQueries.Add(sql);
            }
            
            Assert.Equal(2, sqlQueries.Count);
            Assert.Equal("select NEW.`Id` from NEW union select `EmailNotifications`.`Id` from `EmailNotifications`", sqlQueries[0]);
            Assert.Equal("select NEW.`Id` from NEW union select `TelegramNotifications`.`Id` from `TelegramNotifications`", sqlQueries[1]);
        }

        private static void AddTriggerAction<TEntity>(Trigger<TEntity, NewTableRef<TEntity>> trigger)
            where TEntity : Notification
        {
            trigger.Action(actions => actions.ExecuteRawSql(
                "select {0} from {1} union select {2} from {3}",
                tableRefs => tableRefs.New.Id,
                tableRefs => tableRefs.New,
                _ => TriggerFunctions.GetColumnName((TEntity e) => e.Id),
                _ => TriggerFunctions.GetTableName<TEntity>()));
        }
    }
}