using Laraue.EfCoreTriggers.Common;
using Laraue.EfCoreTriggers.CSharpBuilder;
using Laraue.EfCoreTriggers.Tests.TriggerGeneration.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.Conventions;
using Xunit;

namespace Laraue.EfCoreTriggers.Tests.TriggerGeneration
{
    public class PostgreSqlGenerationTests : TriggerTestBase
    {
        [Fact]
        public void AfterInsert()
        {
            var sql = GetAnnotationSqlFromDbContext<Transaction>(TriggerTime.After, TriggerType.Insert);
            Assert.Equal("CREATE FUNCTION LC_TRIGGER_AFTER_INSERT_TRANSACTION() RETURNS trigger " +
                "as $LC_TRIGGER_AFTER_INSERT_TRANSACTION$ BEGIN IF NEW.is_veryfied is true " +
                "THEN INSERT INTO balances (balance) VALUES (NEW.value) " +
                "ON CONFLICT (user_id) DO UPDATE SET balance = balances.balance + NEW.value;" +
                "UPDATE balances SET balance = balances.balance + NEW.value WHERE balances.user_id = NEW.user_id;" +
                "END IF;RETURN NEW; END;$LC_TRIGGER_AFTER_INSERT_TRANSACTION$ LANGUAGE plpgsql;" +
                "CREATE TRIGGER LC_TRIGGER_AFTER_INSERT_TRANSACTION AFTER INSERT ON transactions " +
                "FOR EACH ROW EXECUTE PROCEDURE LC_TRIGGER_AFTER_INSERT_TRANSACTION();", sql);
        }

        [Fact]
        public void AfterUpdate()
        {
            var sql = GetAnnotationSqlFromDbContext<Transaction>(TriggerTime.After, TriggerType.Update);
            Assert.Equal("CREATE FUNCTION LC_TRIGGER_AFTER_UPDATE_TRANSACTION() RETURNS trigger " +
                "as $LC_TRIGGER_AFTER_UPDATE_TRANSACTION$ BEGIN IF OLD.is_veryfied is true && NEW.is_veryfied is true " +
                "THEN INSERT INTO balances (balance) VALUES (NEW.value - OLD.value) " +
                "ON CONFLICT (user_id) DO UPDATE SET balance = NEW.value - OLD.value + balances.balance;" +
                "UPDATE balances SET balance = balances.balance + NEW.value - OLD.value WHERE balances.user_id = OLD.user_id;" +
                "END IF;RETURN NEW; END;$LC_TRIGGER_AFTER_UPDATE_TRANSACTION$ LANGUAGE plpgsql;" +
                "CREATE TRIGGER LC_TRIGGER_AFTER_UPDATE_TRANSACTION AFTER UPDATE ON transactions " +
                "FOR EACH ROW EXECUTE PROCEDURE LC_TRIGGER_AFTER_UPDATE_TRANSACTION();", sql);
        }

        [Fact]
        public void AfterDelete()
        {
            var sql = GetAnnotationSqlFromDbContext<Transaction>(TriggerTime.After, TriggerType.Delete);
            Assert.Equal("CREATE FUNCTION LC_TRIGGER_AFTER_DELETE_TRANSACTION() RETURNS trigger " +
                "as $LC_TRIGGER_AFTER_DELETE_TRANSACTION$ BEGIN IF OLD.is_veryfied is true " +
                "THEN INSERT INTO balances (balance) VALUES (-OLD.value) " +
                "ON CONFLICT (user_id) DO UPDATE SET balance = balances.balance - OLD.value;" +
                "UPDATE balances SET balance = balances.balance - OLD.value WHERE balances.user_id = OLD.user_id;" +
                "END IF;RETURN NEW; END;$LC_TRIGGER_AFTER_DELETE_TRANSACTION$ LANGUAGE plpgsql;" +
                "CREATE TRIGGER LC_TRIGGER_AFTER_DELETE_TRANSACTION AFTER DELETE ON transactions " +
                "FOR EACH ROW EXECUTE PROCEDURE LC_TRIGGER_AFTER_DELETE_TRANSACTION();", sql);
        }
    }
}
