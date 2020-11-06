using Laraue.EfCoreTriggers.Common.Builders.Providers;
using Laraue.EfCoreTriggers.Common.Builders.Visitor;
using Laraue.EfCoreTriggers.Tests.Entities;
using Xunit;

namespace Laraue.EfCoreTriggers.Tests.TriggerGeneration
{
    public class CreateAndDropTriggerTests : TriggerTestBase
    {
        private readonly ITriggerSqlVisitor _visitor;

        public CreateAndDropTriggerTests()
        {
            _visitor = new PostgreSqlVisitor(DbContext.Model);
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
