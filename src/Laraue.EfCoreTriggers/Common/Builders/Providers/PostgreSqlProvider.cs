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

        protected override string NewEntityPrefix => "NEW";

        protected override string OldEntityPrefix => "OLD";

        public override GeneratedSql GetDropTriggerSql(string triggerName, Type entityType)
        {
            return new GeneratedSql()
                .Append($"DROP TRIGGER {triggerName} ON {GetTableName(entityType)};")
                .Append($"DROP FUNCTION {triggerName}();");
        }

        public override GeneratedSql GetTriggerActionsSql<TTriggerEntity>(TriggerActions<TTriggerEntity> triggerActions)
        {
            var sqlResult = new GeneratedSql();

            if (triggerActions.ActionConditions.Count > 0)
            {
                var conditionsSql = triggerActions.ActionConditions.Select(actionCondition => actionCondition.BuildSql(this));
                sqlResult.MergeColumnsInfo(conditionsSql);
                sqlResult.Append($"IF ")
                    .AppendJoin(" AND ", conditionsSql.Select(x => x.SqlBuilder))
                    .Append($" THEN ");
            }

            var actionsSql = triggerActions.ActionExpressions.Select(action => action.BuildSql(this));
            sqlResult.MergeColumnsInfo(actionsSql)
                .AppendJoin(", ", actionsSql.Select(x => x.SqlBuilder));

            if (triggerActions.ActionConditions.Count > 0)
            {
                sqlResult
                    .Append($"END IF;");
            }

            return sqlResult;
        }

        public override GeneratedSql GetTriggerSql<TTriggerEntity>(Trigger<TTriggerEntity> trigger)
        {
            var triggerTypeName = GetTriggerTypeName(trigger.TriggerType);

            var actionsSql = trigger.Actions.Select(action => action.BuildSql(this));
            return new GeneratedSql(actionsSql)
                .Append($"CREATE FUNCTION {trigger.Name}() RETURNS trigger as ${trigger.Name}$ ")
                .Append("BEGIN ")
                .AppendJoin(actionsSql.Select(x => x.SqlBuilder))
                .Append(" RETURN NEW;END;")
                .Append($"${trigger.Name}$ LANGUAGE plpgsql;")
                .Append($"CREATE TRIGGER {trigger.Name} {triggerTypeName} {trigger.TriggerAction.ToString().ToUpper()} ")
                .Append($"ON {GetTableName(typeof(TTriggerEntity))} FOR EACH ROW EXECUTE PROCEDURE {trigger.Name}();");
        }

        protected override GeneratedSql GetMethodConcatCallExpressionSql(params GeneratedSql[] concatExpressionArgsSql)
            => new GeneratedSql(concatExpressionArgsSql)
                .Append("CONCAT(")
                .AppendJoin(", ", concatExpressionArgsSql.Select(x => x.SqlBuilder))
                .Append(")");
    }
}
