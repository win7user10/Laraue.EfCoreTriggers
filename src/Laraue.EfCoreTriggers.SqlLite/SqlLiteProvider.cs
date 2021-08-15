using System;
using System.Collections.Generic;
using System.Linq;
using Laraue.EfCoreTriggers.Common.Builders.Providers;
using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Concat;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.ToLower;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.ToUpper;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Laraue.EfCoreTriggers.SqlLite
{
    public class SqlLiteProvider : BaseSqlProvider
    {
        public SqlLiteProvider(IModel model) : base(model)
        {
            AddConverter(new ConcatStringViaDoubleVerticalLineConverter());
            AddConverter(new StringToUpperViaUpperFuncConverter());
            AddConverter(new StringToLowerViaLowerFuncConverter());
        }

        protected override Dictionary<Type, string> TypeMappings { get; } = new ()
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
                sqlResult.Append("WHEN ")
                    .AppendJoin(" AND ", conditionsSql.Select(x => x.StringBuilder));
            }

            var actionsSql = triggerActions.ActionExpressions.Select(action => action.BuildSql(this));
            sqlResult.MergeColumnsInfo(actionsSql)
                .Append(" BEGIN ")
                .AppendJoin(", ", actionsSql.Select(x => x.StringBuilder))
                .Append(" END; ");

            return sqlResult;
        }

        public override SqlBuilder GetTriggerSql<TTriggerEntity>(Trigger<TTriggerEntity> trigger)
        {
            var triggerTimeName = GetTriggerTimeName(trigger.TriggerTime);

            var actionsSql = trigger.Actions.Select(action => action.BuildSql(this)).ToArray();
            var generatedSql = new SqlBuilder(actionsSql);

            var actionsCount = actionsSql.Length;
            for (var i = 0; i < actionsCount; i++)
            {
                var postfix = actionsCount > 1 ? $"_{i + 1}" : string.Empty;
                generatedSql.Append($"CREATE TRIGGER {trigger.Name}{postfix} {triggerTimeName} {trigger.TriggerEvent.ToString().ToUpper()} ")
                   .Append($"ON {GetTableName(typeof(TTriggerEntity))} FOR EACH ROW ")
                   .Append(actionsSql[i].StringBuilder);
            }
            return generatedSql;
        }

        protected override string GetNewGuidExpressionSql() =>
            "lower(hex(randomblob(4))) || '-' || lower(hex(randomblob(2))) || '-4' || substr(lower(hex(randomblob(2))),2) || '-' || substr('89ab', abs(random()) % 4 + 1, 1) || substr(lower(hex(randomblob(2))),2) || '-' || lower(hex(randomblob(6)))";
    }
}
