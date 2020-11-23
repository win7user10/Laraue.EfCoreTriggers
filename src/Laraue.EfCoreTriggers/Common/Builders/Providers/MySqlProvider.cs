using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Providers
{
    public class MySqlProvider : SqlLiteProvider
    {
        public MySqlProvider(IModel model) : base(model)
        {
        }

        protected override IEnumerable<TriggerTime> AvailableTriggerTimes { get; } = new[] { TriggerTime.Before, TriggerTime.After };

        public override SqlBuilder GetDropTriggerSql(string triggerName)
        {
            return new SqlBuilder().Append($"DROP TRIGGER {triggerName};");
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
            var triggerTimeName = GetTriggerTimeName(trigger.TriggerTime);
            var actionsSql = trigger.Actions.Select(action => action.BuildSql(this));
            var sql = new SqlBuilder(actionsSql)
                .Append($"CREATE TRIGGER {trigger.Name} {triggerTimeName} {trigger.TriggerEvent.ToString().ToUpper()} ")
                .AppendNewLine($"ON {GetTableName(typeof(TTriggerEntity))} FOR EACH ROW BEGIN ")
                .AppendJoin(SqlBuilder.NewLine, actionsSql.Select(x => x.StringBuilder))
                .AppendNewLine("END ");
            return sql;
        }

        public override SqlBuilder GetTriggerUpsertActionSql<TTriggerEntity, TUpdateEntity>(TriggerUpsertAction<TTriggerEntity, TUpdateEntity> triggerUpsertAction)
        {
            var insertStatementSql = GetInsertStatementBodySql(triggerUpsertAction.InsertExpression, triggerUpsertAction.InsertExpressionPrefixes);
            var newExpressionColumnsSql = GetNewExpressionColumnsSql(
                (NewExpression)triggerUpsertAction.MatchExpression.Body,
                triggerUpsertAction.MatchExpressionPrefixes.ToDictionary(x => x.Key, x => ArgumentType.None));

            var sqlBuilder = new SqlBuilder(insertStatementSql.AffectedColumns)
                .MergeColumnsInfo(newExpressionColumnsSql);

            if (triggerUpsertAction.OnMatchExpression is null)
            {
                sqlBuilder.Append($"INSERT IGNORE {GetTableName(typeof(TUpdateEntity))} ")
                    .Append(insertStatementSql.StringBuilder)
                    .Append(";");
            }
            else
            {
                var updateStatementSql = GetUpdateStatementBodySql(triggerUpsertAction.OnMatchExpression, triggerUpsertAction.OnMatchExpressionPrefixes);
                sqlBuilder.MergeColumnsInfo(updateStatementSql.AffectedColumns)
                    .Append($"INSERT INTO {GetTableName(typeof(TUpdateEntity))} ")
                    .Append(insertStatementSql.StringBuilder)
                    .Append(" ON DUPLICATE KEY UPDATE ")
                    .Append(updateStatementSql.StringBuilder)
                    .Append(";");
            }

            return sqlBuilder;
        }

        protected override SqlBuilder GetMethodConcatCallExpressionSql(params SqlBuilder[] concatExpressionArgsSql)
            => new SqlBuilder(concatExpressionArgsSql)
                .Append("CONCAT(")
                .AppendJoin(", ", concatExpressionArgsSql.Select(x => x.StringBuilder))
                .Append(")");
    }
}
