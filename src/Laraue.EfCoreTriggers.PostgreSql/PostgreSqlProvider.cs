using System;
using System.Collections.Generic;
using System.Linq;
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
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.ToLower;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.ToUpper;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Trim;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Contains;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Microsoft.EntityFrameworkCore.Metadata;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.EndsWith;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.IsNullOrEmpty;

namespace Laraue.EfCoreTriggers.PostgreSql
{
    public class PostgreSqlProvider : BaseSqlProvider
    {
        public PostgreSqlProvider(IReadOnlyModel model) : base(model)
        {
            AddConverter(new ConcatStringViaConcatFuncConverter());
            AddConverter(new StringToUpperViaUpperFuncConverter());
            AddConverter(new StringToLowerViaLowerFuncConverter());
            AddConverter(new StringTrimViaBtrimFuncConverter());
            AddConverter(new StringContainsViaStrposFuncConverter());
            AddConverter(new StringEndsWithViaDoubleVerticalLineConverter());
            AddConverter(new StringIsNullOrEmptyConverter());
            AddConverter(new MathAbsConverter());
            AddConverter(new MathAcosConverter());
            AddConverter(new MathAsinConverter());
            AddConverter(new MathAtanConverter());
            AddConverter(new MathAtan2Converter());
            AddConverter(new MathCeilingConverter());
            AddConverter(new MathCosConverter());
            AddConverter(new MathExpConverter());
            AddConverter(new MathFloorConverter());
        }

        protected override Dictionary<Type, string> TypeMappings { get; } = new ()
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
                var conditionsSql = triggerActions
                    .ActionConditions
                    .Select(actionCondition => actionCondition.BuildSql(this))
                    .ToArray();
                
                sqlResult.MergeColumnsInfo(conditionsSql);
                sqlResult.Append($"IF ")
                    .AppendJoin(" AND ", conditionsSql.Select(x => x.StringBuilder))
                    .Append($" THEN ");
            }

            var actionsSql = triggerActions.ActionExpressions.Select(action => action.BuildSql(this))
                .ToArray();
            
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
            var actionsSql = trigger.Actions.Select(action => action.BuildSql(this))
                .ToArray();
            
            return new SqlBuilder(actionsSql)
                .Append($"CREATE FUNCTION {trigger.Name}() RETURNS trigger as ${trigger.Name}$ ")
                .Append("BEGIN ")
                .AppendJoin(actionsSql.Select(x => x.StringBuilder))
                .Append(" RETURN NEW;END;")
                .Append($"${trigger.Name}$ LANGUAGE plpgsql;")
                .Append($"CREATE TRIGGER {trigger.Name} {GetTriggerTimeName(trigger.TriggerTime)} {trigger.TriggerEvent.ToString().ToUpper()} ")
                .Append($"ON \"{GetTableName(typeof(TTriggerEntity))}\" FOR EACH ROW EXECUTE PROCEDURE {trigger.Name}();");
        }

        protected override string GetNewGuidExpressionSql() => "uuid_generate_v4()";
    }
}
