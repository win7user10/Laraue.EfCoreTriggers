using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Abs;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Acos;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Asin;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Atan;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.AtanTwo;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Ceiling;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Concat;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.ToLower;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.ToUpper;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Trim;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Contains;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.EndsWith;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.IsNullOrEmpty;

namespace Laraue.EfCoreTriggers.MySql
{
    public class MySqlProvider : BaseSqlProvider
    {
        public MySqlProvider(IModel model) : base(model)
        {
            AddConverter(new ConcatStringViaConcatFuncConverter());
            AddConverter(new StringToUpperViaUpperFuncConverter());
            AddConverter(new StringToLowerViaLowerFuncConverter());
            AddConverter(new StringTrimViaTrimFuncConverter());
            AddConverter(new StringContainsViaInstrFuncConverter());
            AddConverter(new StringEndsWithViaConcatFuncConverter());
            AddConverter(new StringIsNullOrEmptyConverter());
            AddConverter(new MathAbsConverter());
            AddConverter(new MathAcosConverter());
            AddConverter(new MathAsinConverter());
            AddConverter(new MathAtanConverter());
            AddConverter(new MathAtanTwoConverter());
            AddConverter(new MathCeilingConverter());
        }

        protected override Dictionary<Type, string> TypeMappings { get; } = new()
        {
            [typeof(bool)] = "BIT(1)",
            [typeof(byte)] = "TINYINT UNSIGNED",
            [typeof(short)] = "SMALLINT",
            [typeof(int)] = "INT",
            [typeof(long)] = "BIGINT",
            [typeof(sbyte)] = "TINYINT",
            [typeof(decimal)] = "DECIMAL",
            [typeof(float)] = "FLOAT",
            [typeof(double)] = "DOUBLE",
            [typeof(Enum)] = "INT",
            [typeof(char)] = "CHAR",
            [typeof(string)] = "TEXT",
            [typeof(DateTime)] = "DATETIME",
            [typeof(TimeSpan)] = "TIME",
            [typeof(Guid)] = "CHAR(36)",
        };

        /// <summary>
        /// Casts type in mysql can be different from column types.
        /// While casting, value is looking in this array then in <see cref="TypeMappings"/>.
        /// </summary>
        private Dictionary<Type, string> CastMappings { get; } = new()
        {
            [typeof(int)] = "UNSIGNED",
            [typeof(bool)] = "BINARY",
            [typeof(float)] = "DECIMAL",
            [typeof(double)] = "DECIMAL",
            [typeof(Enum)] = "UNSIGNED",
            [typeof(char)] = "CHAR",
            [typeof(string)] = "CHAR(512)",
            [typeof(DateTime)] = "DATETIME",
            [typeof(TimeSpan)] = "TIME",
        };

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

        protected override string GetConvertExpressionSql(UnaryExpression unaryExpression, string member)
        {
            if (CastMappings.TryGetValue(unaryExpression.Type, out var sqlType))
            {
                return $"CAST({member} AS {sqlType})";
            }

            return base.GetConvertExpressionSql(unaryExpression, member);
        }

        protected override string GetNewGuidExpressionSql() => "UUID()";
    }
}
