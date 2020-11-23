using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq;

namespace Laraue.EfCoreTriggers.Common.Builders.Providers
{
    public class PostgreSqlProvider : SqlLiteProvider
    {
        public PostgreSqlProvider(IModel model) : base(model)
        {
        }

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
    }
}
