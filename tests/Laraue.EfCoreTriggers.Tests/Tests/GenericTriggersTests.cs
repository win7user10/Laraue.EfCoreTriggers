using System;
using System.Collections.Generic;
using System.Linq;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Common.Functions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;
using Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors;
using Laraue.EfCoreTriggers.MySql.Extensions;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xunit;
using Xunit.Categories;
using ITrigger = Laraue.EfCoreTriggers.Common.TriggerBuilders.Abstractions.ITrigger;

namespace Laraue.EfCoreTriggers.Tests.Tests
{
    [UnitTest]
    public class GenericTriggersTests
    {
        private readonly ITriggerActionVisitorFactory _provider;
        private readonly IModel _model;

        public GenericTriggersTests()
        {
            _model = new DatabaseContext().Model;

            _provider = Helper.GetTriggerActionFactory(
                _model,
                collection => collection.AddMySqlServices());
        }

        private sealed class DatabaseContext : DbContext
        {
            public DatabaseContext()
                : base(new DbContextOptionsBuilder<DatabaseContext>()
                    .UseSqlite("Data Source=:memory:").Options)
            {
            }

            public DbSet<EmailNotification> EmailNotifications { get; set; }
            public DbSet<TelegramNotification> TelegramNotifications { get; set; }
            public DbSet<NotificationLog> NotificationsLog { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.AddGenericTrigger(new NotificationsTrigger());
            }

            private sealed class NotificationsTrigger : GenericTrigger<Notification>
            {
                public override void ApplyTrigger<TImplEntity>(EntityTypeBuilder<TImplEntity> modelBuilder)
                {
                    var genericNotificationType = typeof(TImplEntity).ToString();

                    modelBuilder
                        .AfterInsert(x => x
                            .Action(y => y
                                .Update<NotificationLog>(
                                    (inserted, entities) =>
                                        entities.Id == inserted.New.Id,
                                    (inserted, entities) => new NotificationLog
                                    {
                                        Text = inserted.New.Text,
                                        NotificationType = genericNotificationType,
                                        OriginalId = inserted.New.Id
                                    })));
                }
            }
        }

        [Fact]
        public void Library_ShouldHaveOpportunityToRegisterNonGenericTriggers()
        {
            var types = _model.GetEntityTypes()
                .Where(t => t.ClrType.IsAssignableTo(typeof(Notification)));

            var sqlQueries = new List<string>();

            foreach (var type in types)
            {
                var triggerType = typeof(Trigger<,>).MakeGenericType(
                    type.ClrType,
                    typeof(NewTableRef<>).MakeGenericType(type.ClrType));
                var trigger =
                    (ITrigger)Activator.CreateInstance(triggerType, TriggerEvent.Delete, TriggerTime.After)!;
                AddTriggerAction((dynamic)trigger);

                var sql = _provider.Visit(trigger.Actions[0].ActionExpressions.First(), new VisitedMembers());

                sqlQueries.Add(sql);
            }

            Assert.Equal(2, sqlQueries.Count);
            Assert.Equal(
                "select NEW.`Id` from NEW union select `EmailNotifications`.`Id` from `EmailNotifications`",
                sqlQueries[0]);
            Assert.Equal(
                "select NEW.`Id` from NEW union select `TelegramNotifications`.`Id` from `TelegramNotifications`",
                sqlQueries[1]);
        }

        [Fact]
        public void GenericClasses_ShouldBeConverted_Correct()
        {
            var entityType = _model.FindEntityType(typeof(EmailNotification))!;
            var annotation = entityType.FindAnnotation("LC_TRIGGER_AFTER_INSERT_EMAILNOTIFICATION")!;
            var trigger = annotation.Value as Trigger<EmailNotification, NewTableRef<EmailNotification>>;
            var sql = _provider.Visit(trigger!.Actions[0].ActionExpressions.First(), new VisitedMembers());
            Assert.Equal(
                @"UPDATE `NotificationsLog`
SET `Text` = NEW.`Text`, `NotificationType` = 'Laraue.EfCoreTriggers.Tests.Tests.EmailNotification', `OriginalId` = CAST(NEW.`Id` AS BIGINT)
WHERE `NotificationsLog`.`Id` = NEW.`Id`;", sql);
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


    public abstract class Notification
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }

    public sealed class EmailNotification : Notification
    {
    }
        
    public sealed class TelegramNotification : Notification
    {
    }

    public sealed class NotificationLog
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string NotificationType { get; set; }
        public long OriginalId { get; set; }
    }
}