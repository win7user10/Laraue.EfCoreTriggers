using Laraue.EfCoreTriggers.Common;
using Laraue.EfCoreTriggers.Common.Builders.Providers;
using Laraue.EfCoreTriggers.Common.Builders.Visitor;
using Laraue.EfCoreTriggers.Extensions;
using Laraue.EfCoreTriggers.Tests.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Xunit;

namespace Laraue.EfCoreTriggers.Tests
{
    public class CreateAndDropTriggerTests
    {
        private readonly ITriggerSqlVisitor _visitor;
        private readonly NativeDbContext DbContext;

        public CreateAndDropTriggerTests()
        {
            DbContext = new NativeDbContext(new DbContextOptionsBuilder<NativeDbContext>()
                .UseNpgsql("User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=efcoretriggers;")
                .UseSnakeCaseNamingConvention()
                .UseTriggers().Options);

            _visitor = new PostgreSqlVisitor(DbContext.Model);
        }

        protected string GetAnnotationName<T>(TriggerTime triggerTime, TriggerType triggerType)
            => $"{Constants.AnnotationKey}_{triggerTime.ToString().ToUpper()}_{triggerType.ToString().ToUpper()}_{typeof(T).Name.ToUpper()}";

        protected string GetAnnotationSqlFromDbContext<T>(TriggerTime triggerTime, TriggerType triggerType)
        {
            var entity = DbContext.Model.FindEntityType(typeof(T).FullName);
            var annotationName = GetAnnotationName<T>(triggerTime, triggerType);
            var annotation = entity.GetAnnotation(annotationName);
            return (string)annotation.Value;
        }

        [Fact]
        public void CreateDeleteTriggerQuery()
        {
            var sql = GetAnnotationSqlFromDbContext<Transaction>(TriggerTime.After, TriggerType.Delete);
            Assert.StartsWith("CREATE FUNCTION LC_TRIGGER_AFTER_DELETE_TRANSACTION() RETURNS trigger", sql);
        }

        [Fact]
        public void CreateUpdateTriggerQuery()
        {
            var sql = GetAnnotationSqlFromDbContext<Transaction>(TriggerTime.After, TriggerType.Update);
            Assert.StartsWith("CREATE FUNCTION LC_TRIGGER_AFTER_UPDATE_TRANSACTION() RETURNS trigger", sql);
        }

        [Fact]
        public void CreateInsertTriggerQuery()
        {
            var sql = GetAnnotationSqlFromDbContext<Transaction>(TriggerTime.After, TriggerType.Insert);
            Assert.StartsWith("CREATE FUNCTION LC_TRIGGER_AFTER_INSERT_TRANSACTION() RETURNS trigger", sql);
        }

        [Fact]
        public void DropDeleteTriggerQuery()
        {
            var triggerName = GetAnnotationName<Transaction>(TriggerTime.After, TriggerType.Delete);
            var sql = _visitor.GetDropTriggerSql(triggerName, typeof(Transaction));
            Assert.Equal("DROP TRIGGER LC_TRIGGER_AFTER_DELETE_TRANSACTION ON transactions;DROP FUNCTION LC_TRIGGER_AFTER_DELETE_TRANSACTION();", sql);
        }

        [Fact]
        public void DropUpdateTriggerQuery()
        {
            var triggerName = GetAnnotationName<Transaction>(TriggerTime.After, TriggerType.Update);
            var sql = _visitor.GetDropTriggerSql(triggerName, typeof(Transaction));
            Assert.Equal("DROP TRIGGER LC_TRIGGER_AFTER_UPDATE_TRANSACTION ON transactions;DROP FUNCTION LC_TRIGGER_AFTER_UPDATE_TRANSACTION();", sql);
        }

        [Fact]
        public void DropInsertTriggerQuery()
        {
            var triggerName = GetAnnotationName<Transaction>(TriggerTime.After, TriggerType.Insert);
            var sql = _visitor.GetDropTriggerSql(triggerName, typeof(Transaction));
            Assert.Equal("DROP TRIGGER LC_TRIGGER_AFTER_INSERT_TRANSACTION ON transactions;DROP FUNCTION LC_TRIGGER_AFTER_INSERT_TRANSACTION();", sql);
        }
    }
}
