using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Laraue.EfCoreTriggers.PostgreSqlTests.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<Guid>(nullable: false),
                    role = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "balances",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<Guid>(nullable: false),
                    balance = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_balances", x => x.id);
                    table.ForeignKey(
                        name: "fk_balances_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    value = table.Column<decimal>(nullable: false),
                    is_veryfied = table.Column<bool>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    user_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transactions", x => x.id);
                    table.ForeignKey(
                        name: "fk_transactions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transactions_mirror",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    value = table.Column<decimal>(nullable: false),
                    is_veryfied = table.Column<bool>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    user_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transactions_mirror", x => x.id);
                    table.ForeignKey(
                        name: "fk_transactions_mirror_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_balances_user_id",
                table: "balances",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_transactions_user_id",
                table: "transactions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_mirror_user_id",
                table: "transactions_mirror",
                column: "user_id");

            migrationBuilder.Sql("CREATE FUNCTION LC_TRIGGER_AFTER_DELETE_TRANSACTION() RETURNS trigger as $LC_TRIGGER_AFTER_DELETE_TRANSACTION$ BEGIN IF OLD.is_veryfied is true THEN UPDATE balances SET balance = balances.balance - OLD.value WHERE balances.user_id = OLD.user_id;END IF;DELETE FROM transactions_mirror WHERE OLD.id = transactions_mirror.id; RETURN NEW;END;$LC_TRIGGER_AFTER_DELETE_TRANSACTION$ LANGUAGE plpgsql;CREATE TRIGGER LC_TRIGGER_AFTER_DELETE_TRANSACTION AFTER DELETE ON transactions FOR EACH ROW EXECUTE PROCEDURE LC_TRIGGER_AFTER_DELETE_TRANSACTION();");

            migrationBuilder.Sql("CREATE FUNCTION LC_TRIGGER_AFTER_INSERT_TRANSACTION() RETURNS trigger as $LC_TRIGGER_AFTER_INSERT_TRANSACTION$ BEGIN IF NEW.is_veryfied is true THEN INSERT INTO balances (user_id, balance) VALUES (NEW.user_id, NEW.value) ON CONFLICT (user_id) DO UPDATE SET balance = balances.balance + NEW.value;END IF;IF NEW.is_veryfied is false THEN INSERT INTO balances (user_id, balance) VALUES (NEW.user_id, 0) ON CONFLICT (user_id) DO NOTHING;END IF;INSERT INTO transactions_mirror (id, user_id, is_veryfied, value, description) VALUES (NEW.id, NEW.user_id, NEW.is_veryfied, NEW.value, CONCAT(UPPER(NEW.description), LOWER('-COPY'))); RETURN NEW;END;$LC_TRIGGER_AFTER_INSERT_TRANSACTION$ LANGUAGE plpgsql;CREATE TRIGGER LC_TRIGGER_AFTER_INSERT_TRANSACTION AFTER INSERT ON transactions FOR EACH ROW EXECUTE PROCEDURE LC_TRIGGER_AFTER_INSERT_TRANSACTION();");

            migrationBuilder.Sql("CREATE FUNCTION LC_TRIGGER_AFTER_UPDATE_TRANSACTION() RETURNS trigger as $LC_TRIGGER_AFTER_UPDATE_TRANSACTION$ BEGIN IF OLD.is_veryfied is true AND NEW.is_veryfied is true THEN UPDATE balances SET balance = balances.balance + NEW.value - OLD.value WHERE balances.user_id = OLD.user_id;END IF;IF OLD.is_veryfied is false AND NEW.is_veryfied is true THEN UPDATE balances SET balance = balances.balance + NEW.value WHERE balances.user_id = OLD.user_id;END IF;UPDATE transactions_mirror SET is_veryfied = NEW.is_veryfied, value = NEW.value WHERE transactions_mirror.id = NEW.id; RETURN NEW;END;$LC_TRIGGER_AFTER_UPDATE_TRANSACTION$ LANGUAGE plpgsql;CREATE TRIGGER LC_TRIGGER_AFTER_UPDATE_TRANSACTION AFTER UPDATE ON transactions FOR EACH ROW EXECUTE PROCEDURE LC_TRIGGER_AFTER_UPDATE_TRANSACTION();");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER LC_TRIGGER_AFTER_DELETE_TRANSACTION ON transactions;DROP FUNCTION LC_TRIGGER_AFTER_DELETE_TRANSACTION();");

            migrationBuilder.Sql("DROP TRIGGER LC_TRIGGER_AFTER_INSERT_TRANSACTION ON transactions;DROP FUNCTION LC_TRIGGER_AFTER_INSERT_TRANSACTION();");

            migrationBuilder.Sql("DROP TRIGGER LC_TRIGGER_AFTER_UPDATE_TRANSACTION ON transactions;DROP FUNCTION LC_TRIGGER_AFTER_UPDATE_TRANSACTION();");

            migrationBuilder.DropTable(
                name: "balances");

            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "transactions_mirror");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
