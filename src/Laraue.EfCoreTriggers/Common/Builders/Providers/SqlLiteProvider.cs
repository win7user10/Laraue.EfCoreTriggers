using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Providers
{
    public class SqlLiteProvider : BaseTriggerProvider
    {
        public SqlLiteProvider(IModel model) : base(model)
        {
        }

        protected override string NewEntityPrefix => "NEW";

        protected override string OldEntityPrefix => "OLD";

        public override GeneratedSql GetDropTriggerSql(string triggerName)
        {
            return new GeneratedSql("PRAGMA writable_schema = 1; ")
                .Append($"DELETE FROM sqlite_master WHERE type = 'trigger' AND name like '{triggerName}%';")
                .Append("PRAGMA writable_schema = 0;");
        }

        public override GeneratedSql GetTriggerActionsSql<TTriggerEntity>(TriggerActions<TTriggerEntity> triggerActions)
        {
            var sqlResult = new GeneratedSql();

            if (triggerActions.ActionConditions.Count > 0)
            {
                var conditionsSql = triggerActions.ActionConditions.Select(actionCondition => actionCondition.BuildSql(this));
                sqlResult.MergeColumnsInfo(conditionsSql);
                sqlResult.Append($"WHEN ")
                    .AppendJoin(" AND ", conditionsSql.Select(x => x.SqlBuilder));
            }

            var actionsSql = triggerActions.ActionExpressions.Select(action => action.BuildSql(this));
            sqlResult.MergeColumnsInfo(actionsSql)
                .Append($" BEGIN ")
                .AppendJoin(", ", actionsSql.Select(x => x.SqlBuilder))
                .Append($" END; ");

            return sqlResult;
        }

        public override GeneratedSql GetTriggerSql<TTriggerEntity>(Trigger<TTriggerEntity> trigger)
        {
            var triggerTypeName = GetTriggerTypeName(trigger.TriggerType);

            var actionsSql = trigger.Actions.Select(action => action.BuildSql(this)).ToArray();
            var generatedSql = new GeneratedSql(actionsSql);

            var actionsCount = actionsSql.Length;
            for (int i = 0; i < actionsCount; i++)
            {
                var postfix = actionsCount > 1 ? $"_{i + 1}" : string.Empty;
                generatedSql.Append($"CREATE TRIGGER {trigger.Name}{postfix} {triggerTypeName} {trigger.TriggerAction.ToString().ToUpper()} ")
                   .Append($"ON {GetTableName(typeof(TTriggerEntity))} FOR EACH ROW ")
                   .Append(actionsSql[i].SqlBuilder);
            }
            return generatedSql;
        }

        public override GeneratedSql GetTriggerUpsertActionSql<TTriggerEntity, TUpdateEntity>(TriggerUpsertAction<TTriggerEntity, TUpdateEntity> triggerUpsertAction)
        {
            var insertStatementSql = GetInsertStatementBodySql(triggerUpsertAction.InsertExpression, triggerUpsertAction.InsertExpressionPrefixes);
            var newExpressionColumnsSql = GetNewExpressionColumnsSql(
                (NewExpression)triggerUpsertAction.MatchExpression.Body,
                triggerUpsertAction.MatchExpressionPrefixes.ToDictionary(x => x.Key, x => ArgumentType.None));

            var sqlBuilder = new GeneratedSql(insertStatementSql.AffectedColumns)
                .MergeColumnsInfo(newExpressionColumnsSql)
                .Append($"INSERT INTO {GetTableName(typeof(TUpdateEntity))} ")
                .Append(insertStatementSql.SqlBuilder)
                .Append($" ON CONFLICT (")
                .AppendJoin(", ", newExpressionColumnsSql.Select(x => x.SqlBuilder))
                .Append(")");

            if (triggerUpsertAction.OnMatchExpression is null)
            {
                sqlBuilder.Append(" DO NOTHING;");
            }
            else
            {
                var updateStatementSql = GetUpdateStatementBodySql(triggerUpsertAction.OnMatchExpression, triggerUpsertAction.OnMatchExpressionPrefixes);
                sqlBuilder.MergeColumnsInfo(updateStatementSql.AffectedColumns)
                    .Append($" DO UPDATE SET ")
                    .Append(updateStatementSql.SqlBuilder)
                    .Append(";");
            }

            return sqlBuilder;
        }

        protected override GeneratedSql GetMethodConcatCallExpressionSql(params GeneratedSql[] concatExpressionArgsSql)
            => new GeneratedSql(concatExpressionArgsSql)
                .AppendJoin(" || ", concatExpressionArgsSql.Select(x => x.SqlBuilder));
    }
}
