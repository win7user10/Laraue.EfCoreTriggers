using System;
using System.Collections.Generic;
using System.Linq;
using Laraue.EfCoreTriggers.Common.Builders.Providers;
using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Laraue.EfCoreTriggers.PostgreSql
{
    public class PostgreSqlProvider : SqlLiteProvider
    {
        public PostgreSqlProvider(IModel model) : base(model)
        {
        }

        protected override Dictionary<Type, string> TypeMappings { get; } = new Dictionary<Type, string>
        {
            [typeof(bool)] = "boolean",
            [typeof(byte)] = "smallint",
            [typeof(short)] = "smallint",
            [typeof(int)] = "integer",
            [typeof(long)] = "bigint",
            [typeof(sbyte)] = "smallint",
            [typeof(uint)] = "oid",
            [typeof(decimal)] = "money",
            [typeof(float)] = "real",
            [typeof(double)] = "double precision",
            [typeof(Enum)] = "integer",
            [typeof(char)] = "(internal) char",
            [typeof(string)] = "name",
            [typeof(DateTime)] = "timestamp without time zone",
            [typeof(DateTimeOffset)] = "time with time zone",
            [typeof(TimeSpan)] = "time without time zone",
            [typeof(Guid)] = "uuid",
        };

        public override SqlBuilder GetDropTriggerSql(string triggerName)
        {
            return new SqlBuilder()
                .Append($"DROP FUNCTION {triggerName}() CASCADE;");
        }

        public override SqlBuilder GetTriggerActionsSql<TTriggerEntity>(TriggerActions<TTriggerEntity> triggerActions)
        {
            var sqlResult = new SqlBuilder();

            if (triggerActions.ActionConditions.Count > 0)
            {
                var conditionsSql = triggerActions.ActionConditions.Select(actionCondition => actionCondition.BuildSql(this));
                sqlResult.MergeColumnsInfo(conditionsSql);
                sqlResult.Append($"IF ")
                    .AppendJoin(" AND ", conditionsSql.Select(x => x.StringBuilder))
                    .Append($" THEN ");
            }

            var actionsSql = triggerActions.ActionExpressions.Select(action => action.BuildSql(this));
            sqlResult.MergeColumnsInfo(actionsSql)
                .AppendJoin(", ", actionsSql.Select(x => x.StringBuilder));

            if (triggerActions.ActionConditions.Count > 0)
            {
                sqlResult
                    .Append($"END IF;");
            }

            return sqlResult;
        }

        public override SqlBuilder GetTriggerSql<TTriggerEntity>(Trigger<TTriggerEntity> trigger)
        {
            var actionsSql = trigger.Actions.Select(action => action.BuildSql(this));
            return new SqlBuilder(actionsSql)
                .Append($"CREATE FUNCTION {trigger.Name}() RETURNS trigger as ${trigger.Name}$ ")
                .Append("BEGIN ")
                .AppendJoin(actionsSql.Select(x => x.StringBuilder))
                .Append(" RETURN NEW;END;")
                .Append($"${trigger.Name}$ LANGUAGE plpgsql;")
                .Append($"CREATE TRIGGER {trigger.Name} {GetTriggerTimeName(trigger.TriggerTime)} {trigger.TriggerEvent.ToString().ToUpper()} ")
                .Append($"ON {GetTableName(typeof(TTriggerEntity))} FOR EACH ROW EXECUTE PROCEDURE {trigger.Name}();");
        }

        protected override SqlBuilder GetMethodConcatCallExpressionSql(params SqlBuilder[] concatExpressionArgsSql)
            => new SqlBuilder(concatExpressionArgsSql)
                .Append("CONCAT(")
                .AppendJoin(", ", concatExpressionArgsSql.Select(x => x.StringBuilder))
                .Append(")");

        protected override string GetNewGuidExpressionSql() => "uuid_generate_v4()";
    }
}
