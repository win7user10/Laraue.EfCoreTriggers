using Laraue.EfCoreTriggers;
using Laraue.EfCoreTriggers.Common;
using Laraue.EfCoreTriggers.Common.Builders.Providers;
using Laraue.EfCoreTriggers.Common.Builders.Visitor;
using Laraue.EfCoreTriggers.Tests.StoredProcedures.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Xunit;

namespace Laraue.EfCoreTriggers.Tests.StoredProcedures
{
    public class PostgreTriggersTests
    {
        private readonly TestDbContext _dbContext;
        private readonly ITriggerSqlVisitor _visitor;

        public PostgreTriggersTests()
        {
            _dbContext = new ContextFactory().CreatePgDbContext();
            _visitor = new PostgreSqlVisitor(_dbContext.Model);
        }

        private string GetAnnotationName<T>(TriggerTime triggerTime, TriggerType triggerType)
            => $"{Constants.AnnotationKey}_{triggerTime}_{triggerType}_{typeof(T).Name}";

        private string GetAnnotationSql<T>(TriggerTime triggerTime, TriggerType triggerType)
        {
            var entity = _dbContext.Model.FindEntityType(typeof(T).FullName);
            var annotationName = GetAnnotationName<T>(triggerTime, triggerType);
            var annotation = entity.GetAnnotation(annotationName);
            return (string)annotation.Value;
        }

        [Fact]
        public void CreateDeleteTriggerQuery()
        {
            var sql = GetAnnotationSql<Transaction>(TriggerTime.After, TriggerType.Delete);
            Assert.Equal("CREATE FUNCTION LC_TRIGGER_After_Delete_Transaction() RETURNS trigger " +
                "as $LC_TRIGGER_After_Delete_Transaction$ BEGIN IF OLD.is_veryfied is true " +
                "THEN update transactions set balance = balances.balance - OLD.value " +
                "where balances.user_id = OLD.user_id;END IF;RETURN NEW; END;" +
                "$LC_TRIGGER_After_Delete_Transaction$ LANGUAGE plpgsql;" +
                "CREATE TRIGGER LC_TRIGGER_After_Delete_Transaction AFTER DELETE ON transactions " +
                "FOR EACH ROW EXECUTE PROCEDURE LC_TRIGGER_After_Delete_Transaction();", sql);
        }

        [Fact]
        public void CreateUpdateTriggerQuery()
        {
            var sql = GetAnnotationSql<Transaction>(TriggerTime.After, TriggerType.Update);
            Assert.Equal("CREATE FUNCTION LC_TRIGGER_After_Update_Transaction() RETURNS trigger " +
                "as $LC_TRIGGER_After_Update_Transaction$ BEGIN IF OLD.is_veryfied is true && NEW.is_veryfied is true " +
                "THEN update transactions set balance = balances.balance + NEW.value - OLD.value " +
                "where balances.user_id = OLD.user_id;END IF;RETURN NEW; END;" +
                "$LC_TRIGGER_After_Update_Transaction$ LANGUAGE plpgsql;" +
                "CREATE TRIGGER LC_TRIGGER_After_Update_Transaction AFTER UPDATE ON transactions " +
                "FOR EACH ROW EXECUTE PROCEDURE LC_TRIGGER_After_Update_Transaction();", sql);
        }

        [Fact]
        public void CreateInsertTriggerQuery()
        {
            var sql = GetAnnotationSql<Transaction>(TriggerTime.After, TriggerType.Insert);
            Assert.Equal("CREATE FUNCTION LC_TRIGGER_After_Insert_Transaction() RETURNS trigger " +
                "as $LC_TRIGGER_After_Insert_Transaction$ BEGIN IF NEW.is_veryfied is true " +
                "THEN update transactions set balance = balances.balance + NEW.value " +
                "where balances.user_id = NEW.user_id;END IF;RETURN NEW; END;" +
                "$LC_TRIGGER_After_Insert_Transaction$ LANGUAGE plpgsql;" +
                "CREATE TRIGGER LC_TRIGGER_After_Insert_Transaction AFTER INSERT ON transactions " +
                "FOR EACH ROW EXECUTE PROCEDURE LC_TRIGGER_After_Insert_Transaction();", sql);
        }

        [Fact]
        public void DropDeleteTriggerQuery()
        {
            var triggerName = GetAnnotationName<Transaction>(TriggerTime.After, TriggerType.Delete);
            var sql = _visitor.GetDropTriggerSql(triggerName, typeof(Transaction));
            Assert.Equal("DROP TRIGGER LC_TRIGGER_After_Delete_Transaction ON transactions;DROP FUNCTION LC_TRIGGER_After_Delete_Transaction();", sql);
        }

        [Fact]
        public void DropUpdateTriggerQuery()
        {
            var triggerName = GetAnnotationName<Transaction>(TriggerTime.After, TriggerType.Update);
            var sql = _visitor.GetDropTriggerSql(triggerName, typeof(Transaction));
            Assert.Equal("DROP TRIGGER LC_TRIGGER_After_Update_Transaction ON transactions;DROP FUNCTION LC_TRIGGER_After_Update_Transaction();", sql);
        }

        [Fact]
        public void DropInsertTriggerQuery()
        {
            var triggerName = GetAnnotationName<Transaction>(TriggerTime.After, TriggerType.Insert);
            var sql = _visitor.GetDropTriggerSql(triggerName, typeof(Transaction));
            Assert.Equal("DROP TRIGGER LC_TRIGGER_After_Insert_Transaction ON transactions;DROP FUNCTION LC_TRIGGER_After_Insert_Transaction();", sql);
        }
    }
}
