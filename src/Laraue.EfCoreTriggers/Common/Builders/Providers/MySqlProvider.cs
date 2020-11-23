using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Providers
{
    public class MySqlProvider : SqlLiteProvider
    {
        public MySqlProvider(IModel model) : base(model)
        {
        }

        public override GeneratedSql GetDropTriggerSql(string triggerName)
        {
            return new GeneratedSql().Append($"DROP TRIGGER {triggerName};");
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
            var triggerTimeName = GetTriggerTimeName(trigger.TriggerTime);
            var actionsSql = trigger.Actions.Select(action => action.BuildSql(this));
            var sql = new GeneratedSql(actionsSql)
                .Append($"CREATE TRIGGER {trigger.Name} {triggerTimeName} {trigger.TriggerEvent.ToString().ToUpper()} ")
                .Append($"ON {GetTableName(typeof(TTriggerEntity))} FOR EACH ROW BEGIN ")
                .Append(@"
")
                .AppendJoin(@"
", actionsSql.Select(x => x.SqlBuilder))
                .Append(@"
END ");
            return sql;
        }

        public override GeneratedSql GetTriggerUpsertActionSql<TTriggerEntity, TUpdateEntity>(TriggerUpsertAction<TTriggerEntity, TUpdateEntity> triggerUpsertAction)
        {
            var insertStatementSql = GetInsertStatementBodySql(triggerUpsertAction.InsertExpression, triggerUpsertAction.InsertExpressionPrefixes);
            var newExpressionColumnsSql = GetNewExpressionColumnsSql(
                (NewExpression)triggerUpsertAction.MatchExpression.Body,
                triggerUpsertAction.MatchExpressionPrefixes.ToDictionary(x => x.Key, x => ArgumentType.None));

            var sqlBuilder = new GeneratedSql(insertStatementSql.AffectedColumns)
                .MergeColumnsInfo(newExpressionColumnsSql);

            if (triggerUpsertAction.OnMatchExpression is null)
            {
                sqlBuilder.Append($"INSERT IGNORE {GetTableName(typeof(TUpdateEntity))} ")
                    .Append(insertStatementSql.SqlBuilder)
                    .Append(";");
            }
            else
            {
                var updateStatementSql = GetUpdateStatementBodySql(triggerUpsertAction.OnMatchExpression, triggerUpsertAction.OnMatchExpressionPrefixes);
                sqlBuilder.MergeColumnsInfo(updateStatementSql.AffectedColumns)
                    .Append($"INSERT INTO {GetTableName(typeof(TUpdateEntity))} ")
                    .Append(insertStatementSql.SqlBuilder)
                    .Append(" ON DUPLICATE KEY UPDATE ")
                    .Append(updateStatementSql.SqlBuilder)
                    .Append(";");
            }

            return sqlBuilder;
        }

        protected override GeneratedSql GetMethodConcatCallExpressionSql(params GeneratedSql[] concatExpressionArgsSql)
            => new GeneratedSql(concatExpressionArgsSql)
                .Append("CONCAT(")
                .AppendJoin(", ", concatExpressionArgsSql.Select(x => x.SqlBuilder))
                .Append(")");
    }
}
