using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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

namespace Laraue.EfCoreTriggers.SqlServer
{
    public class SqlServerProvider : BaseSqlProvider
    {
        public SqlServerProvider(IModel model) : base(model)
        {
            AddConverter(new ConcatStringViaPlusConverter());
            AddConverter(new StringToUpperViaUpperFuncConverter());
            AddConverter(new StringToLowerViaLowerFuncConverter());
            AddConverter(new StringTrimViaLtrimRtrimFuncConverter());
            AddConverter(new StringContainsViaCharindexFuncConverter());
            AddConverter(new StringEndsWithViaPlusConverter());
            AddConverter(new StringIsNullOrEmptyConverter());
            AddConverter(new MathAbsConverter());
            AddConverter(new MathAcosConverter());
            AddConverter(new MathAsinConverter());
            AddConverter(new MathAtanConverter());
            AddConverter(new MathAtn2Converter());
            AddConverter(new MathCeilingConverter());
            AddConverter(new MathCosConverter());
            AddConverter(new MathExpConverter());
            AddConverter(new MathFloorConverter());
        }

        protected override Dictionary<Type, string> TypeMappings { get; } = new()
        {
            [typeof(bool)] = "BIT",
            [typeof(byte)] = "TINYINT",
            [typeof(short)] = "SMALLINT",
            [typeof(int)] = "INT",
            [typeof(long)] = "BIGINT",
            [typeof(sbyte)] = "SMALLMONEY",
            [typeof(ushort)] = "NUMERIC(20)",
            [typeof(uint)] = "NUMERIC(28)",
            [typeof(ulong)] = "NUMERIC(29)",
            [typeof(decimal)] = "DECIMAL(18,2)",
            [typeof(float)] = "FLOAT(24)",
            [typeof(double)] = "FLOAT",
            [typeof(Enum)] = "INT",
            [typeof(char)] = "CHAR(1)",
            [typeof(string)] = "NVARCHAR(MAX)",
            [typeof(DateTime)] = "DATETIME2",
            [typeof(DateTimeOffset)] = "DATETIMEOFFSET",
            [typeof(TimeSpan)] = "TIME",
            [typeof(Guid)] = "UNIQUEIDENTIFIER",
        };

        protected override string NewEntityPrefix => "Inserted";

        protected override string OldEntityPrefix => "Deleted";

        protected override IEnumerable<TriggerTime> AvailableTriggerTimes { get; } =
            new[] { TriggerTime.After, TriggerTime.InsteadOf };

        public override SqlBuilder GetDropTriggerSql(string triggerName)
            => new($"DROP TRIGGER {triggerName};");

        public override SqlBuilder GetTriggerSql<TTriggerEntity>(Trigger<TTriggerEntity> trigger)
        {
            var triggerTimeName = GetTriggerTimeName(trigger.TriggerTime);

            var actionsSql = trigger
                .Actions
                .Select(action => action.BuildSql(this))
                .ToArray();

            var sqlBuilder = new SqlBuilder(actionsSql);
            sqlBuilder.Append($"CREATE TRIGGER {trigger.Name} ON {GetTableName(typeof(TTriggerEntity))} ")
                .Append(triggerTimeName)
                .Append($" {trigger.TriggerEvent} AS BEGIN ");

            if (sqlBuilder.AffectedColumns.Any(x => x.Value.Any()))
            {
                sqlBuilder.Append(DeclareVariablesSql<TTriggerEntity>(sqlBuilder.AffectedColumns))
                    .Append(" ");
            }

            sqlBuilder.Append(DeclareCursorsSql<TTriggerEntity>(sqlBuilder.AffectedColumns, trigger.TriggerEvent))
                .Append(" ")
                .Append(FetchCursorsSql<TTriggerEntity>(sqlBuilder.AffectedColumns, trigger.TriggerEvent))
                .Append(" WHILE @@FETCH_STATUS = 0")
                .Append(" BEGIN ")
                .AppendJoin(actionsSql.Select(x => x.StringBuilder))
                .Append(FetchCursorsSql<TTriggerEntity>(sqlBuilder.AffectedColumns, trigger.TriggerEvent))
                .Append(" END ");

            sqlBuilder.Append(CloseCursorsSql<TTriggerEntity>(sqlBuilder.AffectedColumns, trigger.TriggerEvent))
                .Append(" END");

            return sqlBuilder;
        }

        protected override string GetExpressionOperandSql(Expression expression) => expression.NodeType switch
        {
            ExpressionType.IsTrue => $"= {GetBooleanSqlValue(true)}",
            ExpressionType.IsFalse => $"= {GetBooleanSqlValue(false)}",
            ExpressionType.Not => $"= {GetBooleanSqlValue(false)}",
            _ => base.GetExpressionOperandSql(expression),
        };

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
                sqlResult.Append($"IF (")
                    .AppendJoin(" AND ", conditionsSql.Select(x => x.StringBuilder))
                    .Append($") ");
            }

            var actionsSql = triggerActions
                .ActionExpressions
                .Select(action => action.BuildSql(this))
                .ToArray();

            sqlResult.MergeColumnsInfo(actionsSql)
                .Append("BEGIN ")
                .AppendJoin("; ", actionsSql.Select(x => x.StringBuilder))
                .Append(" END ");

            return sqlResult;
        }

        private string CursorName<TTriggerEntity>(ArgumentType argumentType)
            => $"{TemporaryTableName(argumentType)}{typeof(TTriggerEntity).Name}Cursor";

        private string TemporaryTableName(ArgumentType argumentType)
        {
            return argumentType switch
            {
                ArgumentType.New => NewEntityPrefix,
                ArgumentType.Old => OldEntityPrefix,
                _ => throw new InvalidOperationException($"Temporary table for {argumentType} is not exists")
            };
        }

        private string DeclareCursorSql(string cursorName)
            => $"DECLARE {cursorName} CURSOR FOR";

        private string FetchCursorsSql<TTriggerEntity>(Dictionary<ArgumentType, HashSet<MemberInfo>> members, TriggerEvent triggerEvent)
        {
            return GetCursorBlockSql<TTriggerEntity>(members, triggerEvent, FetchCursorSql<TTriggerEntity>);
        }

        private SqlBuilder FetchCursorSql<TTriggerEntity>(ArgumentType argumentType, MemberInfo[] members)
        {
            var sqlBuilder = new SqlBuilder($"FETCH NEXT FROM {CursorName<TTriggerEntity>(argumentType)}");

            members = members.WhereDeclaringType<TTriggerEntity>().ToArray();
            
            if (members.Any())
            {
                sqlBuilder.Append(" INTO ")
                    .AppendJoin(", ", members.Select(member => VariableNameSql(argumentType, member)));
            }

            return sqlBuilder;
        }

        private string SelectFromCursorSql<TTriggerEntity>(ArgumentType argumentType, MemberInfo[] members)
        {
            members = members.WhereDeclaringType<TTriggerEntity>().ToArray();
            
            var columns = members.Any()
                ? string.Join(", ", members.Select(GetColumnName))
                : "*";
            
            return $"SELECT {columns} FROM {TemporaryTableName(argumentType)}";
        }

        private string[] DeclareVariablesSql(ArgumentType argumentType, IEnumerable<MemberInfo> members)
        {
            return members.Select(member => DeclareVariableNameSql(argumentType, member))
                .ToArray();
        }
        
        private string DeclareVariablesSql<TTriggerEntity>(Dictionary<ArgumentType, HashSet<MemberInfo>> members)
        {
            var sqlBuilder = new SqlBuilder();
            
            var variablesSql = members
                .ToDictionary(x => x.Key, x => x.Value.WhereDeclaringType<TTriggerEntity>())
                .Where(x => x.Value.Any())
                .SelectMany(x => DeclareVariablesSql(x.Key, x.Value))
                .ToArray();

            if (variablesSql.Any())
            {
                sqlBuilder.Append("DECLARE ")
                    .AppendJoin(", ", variablesSql);
            }
            
            return sqlBuilder;
        }

        private string CloseCursorSql(string cursorName)
            => $"CLOSE {cursorName}";

        private string DeallocateCursorSql(string cursorName)
            => $"DEALLOCATE {cursorName}";

        private SqlBuilder CloseCursorsSql<TTriggerEntity>(Dictionary<ArgumentType, HashSet<MemberInfo>> members, TriggerEvent triggerEvent)
        {
            return GetCursorBlockSql<TTriggerEntity>(members, triggerEvent, (type, _) =>
            {
                var sqlBuilder = new SqlBuilder(CloseCursorSql(CursorName<TTriggerEntity>(type)))
                    .Append(" ")
                    .Append(DeallocateCursorSql(CursorName<TTriggerEntity>(type)));

                return sqlBuilder;
            });
        }

        private string VariableNameSql(ArgumentType argumentType, MemberInfo member)
            => argumentType switch
            {
                ArgumentType.New => $"@New{member.Name}",
                ArgumentType.Old => $"@Old{member.Name}",
                _ => throw new InvalidOperationException(
                    $"Invalid attempt to generate declaring variable SQL using argument prefix {argumentType}")
            };

        private string DeclareVariableNameSql(ArgumentType argumentType, MemberInfo member)
            => $"{VariableNameSql(argumentType, member)} {GetSqlServerType((PropertyInfo)member)}";

        private string GetSqlServerType(PropertyInfo propertyInfo)
            => GetSqlType(propertyInfo.PropertyType) ??
               throw new NotSupportedException($"Unknown data type {propertyInfo.PropertyType}");

        private SqlBuilder DeclareCursorsSql<TTriggerEntity>(Dictionary<ArgumentType, HashSet<MemberInfo>> affectedMemberPairs, TriggerEvent triggerEvent)
        {
            return GetCursorBlockSql<TTriggerEntity>(affectedMemberPairs, triggerEvent, DeclareCursorBlockSql<TTriggerEntity>);
        }

        private SqlBuilder GetCursorBlockSql<TTriggerEntity>(Dictionary<ArgumentType, HashSet<MemberInfo>> affectedMemberPairs, 
            TriggerEvent triggerEvent, 
            Func<ArgumentType, MemberInfo[], SqlBuilder> generateSqlFunction)
        {
            var cursorBlocksSql = affectedMemberPairs
                .Where(x => x.Value.WhereDeclaringType<TTriggerEntity>().Any())
                .Select(x => generateSqlFunction(x.Key, x.Value.ToArray()))
                .ToArray();

            // Union and return general sql for all cursors
            if (cursorBlocksSql.Any())
            {
                return new SqlBuilder()
                    .AppendJoin(" ", cursorBlocksSql.Select(x => x.StringBuilder));
            }
            
            // Generate one cursor just for iterate triggered entities
            var argumentType = triggerEvent == TriggerEvent.Delete
                ? ArgumentType.Old
                : ArgumentType.New;

            return generateSqlFunction(argumentType, Array.Empty<MemberInfo>());
        }

        private SqlBuilder DeclareCursorBlockSql<TTriggerEntity>(ArgumentType argumentType, MemberInfo[] affectedMembers)
        {
            var cursorName = CursorName<TTriggerEntity>(argumentType);
            var sqlBuilder =  new SqlBuilder()
                .Append(DeclareCursorSql(cursorName))
                .Append(" ")
                .Append(SelectFromCursorSql<TTriggerEntity>(argumentType, affectedMembers));

            sqlBuilder.Append($" OPEN {cursorName}");

            return sqlBuilder;
        }

        public override SqlBuilder GetTriggerUpsertActionSql<TTriggerEntity, TUpsertEntity>(
            TriggerUpsertAction<TTriggerEntity, TUpsertEntity> triggerUpsertAction)
        {
            var matchExpressionParts = GetLambdaNewExpressionParts(triggerUpsertAction.MatchExpression, triggerUpsertAction.MatchExpressionPrefixes);

            var sqlBuilder = new SqlBuilder(matchExpressionParts.Select(x => x.Value));
            var insertStatementSql = GetInsertStatementBodySql(triggerUpsertAction.InsertExpression, triggerUpsertAction.InsertExpressionPrefixes);

            sqlBuilder.MergeColumnsInfo(insertStatementSql)
                .Append("BEGIN TRANSACTION;");
            
            if (triggerUpsertAction.OnMatchExpression != null)
            {
                var updateStatementSql = GetUpdateStatementBodySql(triggerUpsertAction.OnMatchExpression, triggerUpsertAction.OnMatchExpressionPrefixes);
                
                sqlBuilder.MergeColumnsInfo(updateStatementSql)
                    .Append($"UPDATE {GetTableName(typeof(TUpsertEntity))} WITH (UPDLOCK, SERIALIZABLE) SET ")
                    .Append(updateStatementSql.StringBuilder)
                    .Append(" WHERE ")
                    .AppendJoin(" AND ", matchExpressionParts
                        .Select(memberPair => $"{GetColumnSql(memberPair.Key, ArgumentType.Default)} = {memberPair.Value}"))
                    .Append($" IF @@ROWCOUNT = 0");
            }
            else
            {
                sqlBuilder.Append($"IF NOT EXISTS(SELECT 1 FROM {GetTableName(typeof(TUpsertEntity))} WITH (UPDLOCK, SERIALIZABLE) WHERE ")
                    .AppendJoin(" AND ", matchExpressionParts
                        .Select(memberPair => $"{GetColumnSql(memberPair.Key, ArgumentType.Default)} = {memberPair.Value}"))
                    .Append(")");
            }

            sqlBuilder
                .Append($" BEGIN INSERT INTO {GetTableName(typeof(TUpsertEntity))} ")
                .Append(insertStatementSql.StringBuilder)
                .Append("; END COMMIT TRANSACTION;");

            return sqlBuilder;
        }

        protected override string GetMemberExpressionSql(MemberExpression memberExpression, ArgumentType argumentType)
        {
            var memberInfo = memberExpression.Member;
            return argumentType switch
            {
                ArgumentType.New => VariableNameSql(argumentType, memberInfo),
                ArgumentType.Old => VariableNameSql(argumentType, memberInfo),
                _ => base.GetMemberExpressionSql(memberExpression, argumentType)
            };
        }

        protected override string GetBooleanSqlValue(bool value) => value
            ? "1"
            : "0";

        protected override string GetNewGuidExpressionSql() => "NEWID()";

        private static readonly HashSet<ExpressionType> BooleanExpressionTypes = new ()
        {
            ExpressionType.Not,
            ExpressionType.Equal
        };

        private bool WasAnyChildCastedToBoolean(UnaryExpression unaryExpression)
        {
            return unaryExpression.Operand is UnaryExpression innerUnaryExpression &&
                   ShouldBeCastedToBoolean(innerUnaryExpression);
        }
        
        private bool ShouldBeCastedToBoolean(UnaryExpression expression)
        {
            if (WasAnyChildCastedToBoolean(expression))
            {
                return false;
            }
            
            if (BooleanExpressionTypes.Contains(expression.NodeType))
            {
                return true;
            }

            return expression.NodeType is ExpressionType.Convert &&
                   GetNotNullableType(expression.Operand.Type) == typeof(bool);
        }

        protected override SqlBuilder GetUnaryExpressionSql(UnaryExpression unaryExpression, Dictionary<string, ArgumentType> argumentTypes)
        {
            var unarySql = base.GetUnaryExpressionSql(unaryExpression, argumentTypes);
            
            if (ShouldBeCastedToBoolean(unaryExpression))
            {
                unarySql.Prepend("CASE WHEN ")
                    .Append(" THEN 1 ELSE 0 END");
            }

            return unarySql;
        }
    }

    internal static class SqlServerProviderExtensions
    {
        public static IEnumerable<MemberInfo> WhereDeclaringType<T>(this IEnumerable<MemberInfo> values)
            => values.Where(x => x.DeclaringType.IsAssignableFrom(typeof(T)));
    }
}