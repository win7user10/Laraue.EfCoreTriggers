using Microsoft.EntityFrameworkCore.Migrations;

namespace Laraue.EfCoreTriggers.PostgreSqlTests.Migrations
{
    public partial class InitTriggers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE FUNCTION LC_TRIGGER_AFTER_DELETE_TRANSACTION() RETURNS trigger as $LC_TRIGGER_AFTER_DELETE_TRANSACTION$ BEGIN IF OLD.is_veryfied is true THEN UPDATE balances SET balance = balances.balance - OLD.value WHERE balances.user_id = OLD.user_id;END IF;RETURN NEW; END;$LC_TRIGGER_AFTER_DELETE_TRANSACTION$ LANGUAGE plpgsql;CREATE TRIGGER LC_TRIGGER_AFTER_DELETE_TRANSACTION AFTER DELETE ON transactions FOR EACH ROW EXECUTE PROCEDURE LC_TRIGGER_AFTER_DELETE_TRANSACTION();");

            migrationBuilder.Sql("CREATE FUNCTION LC_TRIGGER_AFTER_INSERT_TRANSACTION() RETURNS trigger as $LC_TRIGGER_AFTER_INSERT_TRANSACTION$ BEGIN IF NEW.is_veryfied is true THEN INSERT INTO balances (user_id, balance) VALUES (NEW.user_id, NEW.value) ON CONFLICT (user_id) DO UPDATE SET balance = balances.balance + NEW.value;END IF;RETURN NEW; END;$LC_TRIGGER_AFTER_INSERT_TRANSACTION$ LANGUAGE plpgsql;CREATE TRIGGER LC_TRIGGER_AFTER_INSERT_TRANSACTION AFTER INSERT ON transactions FOR EACH ROW EXECUTE PROCEDURE LC_TRIGGER_AFTER_INSERT_TRANSACTION();");

            migrationBuilder.Sql("CREATE FUNCTION LC_TRIGGER_AFTER_UPDATE_TRANSACTION() RETURNS trigger as $LC_TRIGGER_AFTER_UPDATE_TRANSACTION$ BEGIN IF OLD.is_veryfied is true && NEW.is_veryfied is true THEN UPDATE balances SET balance = balances.balance + NEW.value - OLD.value WHERE balances.user_id = OLD.user_id;END IF;RETURN NEW;IF !OLD.is_veryfied && NEW.is_veryfied THEN UPDATE balances SET balance = balances.balance + NEW.value WHERE balances.user_id = OLD.user_id;END IF;RETURN NEW;IF OLD.is_veryfied && !NEW.is_veryfied THEN UPDATE balances SET balance = balances.balance - OLD.value WHERE balances.user_id = OLD.user_id;END IF;RETURN NEW; END;$LC_TRIGGER_AFTER_UPDATE_TRANSACTION$ LANGUAGE plpgsql;CREATE TRIGGER LC_TRIGGER_AFTER_UPDATE_TRANSACTION AFTER UPDATE ON transactions FOR EACH ROW EXECUTE PROCEDURE LC_TRIGGER_AFTER_UPDATE_TRANSACTION();");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER LC_TRIGGER_AFTER_DELETE_TRANSACTION ON transactions;DROP FUNCTION LC_TRIGGER_AFTER_DELETE_TRANSACTION();");

            migrationBuilder.Sql("DROP TRIGGER LC_TRIGGER_AFTER_INSERT_TRANSACTION ON transactions;DROP FUNCTION LC_TRIGGER_AFTER_INSERT_TRANSACTION();");

            migrationBuilder.Sql("DROP TRIGGER LC_TRIGGER_AFTER_UPDATE_TRANSACTION ON transactions;DROP FUNCTION LC_TRIGGER_AFTER_UPDATE_TRANSACTION();");
        }
    }
}
