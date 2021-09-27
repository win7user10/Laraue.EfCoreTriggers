using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Abs;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Acos;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Asin;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Atan;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Atan2;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Ceiling;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Cos;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Exp;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Floor;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Concat;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Contains;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.EndsWith;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.IsNullOrEmpty;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.ToLower;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.ToUpper;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Trim;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
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
            AddConverter(new StringTrimViaTrimFuncConverter());
            AddConverter(new StringContainsViaInstrFuncConverter());
            AddConverter(new StringEndsWithViaDoubleVerticalLineConverter());
            AddConverter(new StringIsNullOrEmptyConverter());
            AddConverter(new MathAbsConverter());
            AddConverter(new MathAcosConverter());
            AddConverter(new MathAsinConverter());
            AddConverter(new MathAtanConverter());
            AddConverter(new MathAtan2Converter());
            AddConverter(new MathCeilConverter());
            AddConverter(new MathCosConverter());
            AddConverter(new MathExpConverter());
            AddConverter(new MathFloorConverter());
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
                var conditionsSql = triggerActions
                    .ActionConditions
                    .Select(actionCondition => actionCondition.BuildSql(this))
                    .ToArray();
                
                sqlResult.MergeColumnsInfo(conditionsSql);
                sqlResult.Append("WHEN ")
                    .AppendJoin(" AND ", conditionsSql.Select(x => x.StringBuilder));
            }

            var actionsSql = triggerActions
                .ActionExpressions
                .Select(action => action.BuildSql(this))
                .ToArray();
            
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
            
            // Reverse trigger actions to fire it in the order set while trigger configuring
            for (var i = actionsCount; i > 0; i--)
            {
                var postfix = actionsCount > 1 ? $"_{actionsCount - i}" : string.Empty;
                generatedSql.Append($"CREATE TRIGGER {trigger.Name}{postfix} {triggerTimeName} {trigger.TriggerEvent.ToString().ToUpper()} ")
                   .Append($"ON {GetTableName(typeof(TTriggerEntity))} FOR EACH ROW ")
                   .Append(actionsSql[i - 1].StringBuilder);
            }
            return generatedSql;
        }

        protected override SqlBuilder GetEmptyInsertStatementBodySql(LambdaExpression insertExpression, Dictionary<string, ArgumentType> argumentTypes)
        {
            var primaryKeyProperties = GetPrimaryKeyMembers(insertExpression.Body.Type);
            var sqlBuilder = new SqlBuilder();
            sqlBuilder.Append("(")
                .AppendJoin(", ", primaryKeyProperties.Select(GetColumnName))
                .Append(") VALUES (")
                .AppendJoin(", ", primaryKeyProperties.Select(_ => "NULL"))
                .Append(")");

            return sqlBuilder;
        }

        protected override string GetNewGuidExpressionSql() =>
            "lower(hex(randomblob(4))) || '-' || lower(hex(randomblob(2))) || '-4' || substr(lower(hex(randomblob(2))),2) || '-' || substr('89ab', abs(random()) % 4 + 1, 1) || substr(lower(hex(randomblob(2))),2) || '-' || lower(hex(randomblob(6)))";
    }
}
