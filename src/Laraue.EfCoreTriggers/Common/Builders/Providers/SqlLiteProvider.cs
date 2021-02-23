using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Providers
{
    public class SqlLiteProvider : BaseTriggerProvider
    {
        public SqlLiteProvider(IModel model) : base(model)
        {
        }

        protected override Dictionary<Type, string> TypeMappings { get; } = new Dictionary<Type, string>
        {
            [typeof(bool)] = "NUMERIC",
            [typeof(byte)] = "NUMERIC",
            [typeof(short)] = "NUMERIC",
            [typeof(int)] = "NUMERIC",
            [typeof(long)] = "NUMERIC",
            [typeof(sbyte)] = "NUMERIC",
            [typeof(uint)] = "NUMERIC",
            [typeof(decimal)] = "NUMERIC",
            [typeof(float)] = "REAL",
            [typeof(double)] = "REAL",
            [typeof(Enum)] = "NUMERIC",
            [typeof(char)] = "TEXT",
            [typeof(string)] = "TEXT",
            [typeof(DateTime)] = "TEXT",
            [typeof(DateTimeOffset)] = "TEXT",
            [typeof(TimeSpan)] = "TEXT",
            [typeof(Guid)] = "TEXT",
        };

        public override SqlBuilder GetDropTriggerSql(string triggerName)
        {
            return new SqlBuilder("PRAGMA writable_schema = 1; ")
                .Append($"DELETE FROM sqlite_master WHERE type = 'trigger' AND name like '{triggerName}%';")
                .Append("PRAGMA writable_schema = 0;");
        }

        public override SqlBuilder GetTriggerActionsSql<TTriggerEntity>(TriggerActions<TTriggerEntity> triggerActions)
        {
            var sqlResult = new SqlBuilder();

            if (triggerActions.ActionConditions.Count > 0)
            {
                var conditionsSql = triggerActions.ActionConditions.Select(actionCondition => actionCondition.BuildSql(this));
                sqlResult.MergeColumnsInfo(conditionsSql);
                sqlResult.Append($"WHEN ")
                    .AppendJoin(" AND ", conditionsSql.Select(x => x.StringBuilder));
            }

            var actionsSql = triggerActions.ActionExpressions.Select(action => action.BuildSql(this));
            sqlResult.MergeColumnsInfo(actionsSql)
                .Append($" BEGIN ")
                .AppendJoin(", ", actionsSql.Select(x => x.StringBuilder))
                .Append($" END; ");

            return sqlResult;
        }

        public override SqlBuilder GetTriggerSql<TTriggerEntity>(Trigger<TTriggerEntity> trigger)
        {
            var triggerTimeName = GetTriggerTimeName(trigger.TriggerTime);

            var actionsSql = trigger.Actions.Select(action => action.BuildSql(this)).ToArray();
            var generatedSql = new SqlBuilder(actionsSql);

            var actionsCount = actionsSql.Length;
            for (int i = 0; i < actionsCount; i++)
            {
                var postfix = actionsCount > 1 ? $"_{i + 1}" : string.Empty;
                generatedSql.Append($"CREATE TRIGGER {trigger.Name}{postfix} {triggerTimeName} {trigger.TriggerEvent.ToString().ToUpper()} ")
                   .Append($"ON {GetTableName(typeof(TTriggerEntity))} FOR EACH ROW ")
                   .Append(actionsSql[i].StringBuilder);
            }
            return generatedSql;
        }

        public override SqlBuilder GetTriggerUpsertActionSql<TTriggerEntity, TUpdateEntity>(TriggerUpsertAction<TTriggerEntity, TUpdateEntity> triggerUpsertAction)
        {
            var insertStatementSql = GetInsertStatementBodySql(triggerUpsertAction.InsertExpression, triggerUpsertAction.InsertExpressionPrefixes);
            var newExpressionColumnsSql = GetNewExpressionColumnsSql(
                (NewExpression)triggerUpsertAction.MatchExpression.Body,
                triggerUpsertAction.MatchExpressionPrefixes.ToDictionary(x => x.Key, x => ArgumentType.None));

            var sqlBuilder = new SqlBuilder(insertStatementSql.AffectedColumns)
                .MergeColumnsInfo(newExpressionColumnsSql)
                .Append($"INSERT INTO {GetTableName(typeof(TUpdateEntity))} ")
                .Append(insertStatementSql.StringBuilder)
                .Append($" ON CONFLICT (")
                .AppendJoin(", ", newExpressionColumnsSql.Select(x => x.StringBuilder))
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
                    .Append(updateStatementSql.StringBuilder)
                    .Append(";");
            }

            return sqlBuilder;
        }

        protected override SqlBuilder GetMethodConcatCallExpressionSql(params SqlBuilder[] concatExpressionArgsSql)
            => new SqlBuilder(concatExpressionArgsSql)
                .AppendJoin(" || ", concatExpressionArgsSql.Select(x => x.StringBuilder));

        protected override string GetNewGuidExpressionSql() =>
            "lower(hex(randomblob(4))) || '-' || lower(hex(randomblob(2))) || '-4' || substr(lower(hex(randomblob(2))),2) || '-' || substr('89ab', abs(random()) % 4 + 1, 1) || substr(lower(hex(randomblob(2))),2) || '-' || lower(hex(randomblob(6)))";
    }
}
