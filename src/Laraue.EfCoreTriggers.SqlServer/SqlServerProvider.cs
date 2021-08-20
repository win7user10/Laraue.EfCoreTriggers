using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Concat;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Contains;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.EndsWith;
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
            AddConverter(new StringTrimViaTrimFuncConverter());
            AddConverter(new StringContainsViaCharindexFuncConverter());
            AddConverter(new StringEndsWithViaPlusConverter());
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
            [typeof(decimal)] = "DECIMAL(38)",
            [typeof(float)] = "FLOAT(24)",
            [typeof(double)] = "FLOAT(53)",
            [typeof(Enum)] = "INT",
            [typeof(char)] = "CHAR(1)",
            [typeof(string)] = "VARCHAR(MAX)",
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

            sqlBuilder.Append(DeclareCursorBlocksSql<TTriggerEntity>(sqlBuilder.AffectedColumns))
                .Append(" ")
                .Append(FetchCursorsSql<TTriggerEntity>(sqlBuilder.AffectedColumns))
                .Append(" WHILE @@FETCH_STATUS = 0")
                .Append(" BEGIN ")
                .AppendJoin(actionsSql.Select(x => x.StringBuilder))
                .Append(FetchCursorsSql<TTriggerEntity>(sqlBuilder.AffectedColumns))
                .Append(" END ");

            sqlBuilder.Append(CloseCursorsBlockSql<TTriggerEntity>(sqlBuilder.AffectedColumns))
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

        private string FetchCursorsSql<TTriggerEntity>(Dictionary<ArgumentType, HashSet<MemberInfo>> members)
            => string.Join(" ",
                members.Where(x => x.Value.WhereDeclaringType<TTriggerEntity>().Any())
                    .Select(x => FetchCursorSql<TTriggerEntity>(x.Key, x.Value)));

        private string FetchCursorSql<TTriggerEntity>(ArgumentType argumentType, IEnumerable<MemberInfo> members)
            =>
                $"FETCH NEXT FROM {CursorName<TTriggerEntity>(argumentType)} INTO {string.Join(", ", members.WhereDeclaringType<TTriggerEntity>().Select(member => VariableNameSql(argumentType, member)))}";

        private string SelectFromCursorSql<TTriggerEntity>(ArgumentType argumentType, IEnumerable<MemberInfo> members)
            =>
                $"SELECT {string.Join(", ", members.WhereDeclaringType<TTriggerEntity>().Select(GetColumnName))} FROM {TemporaryTableName(argumentType)}";

        private string DeclareCursorVariablesSql<TTriggerEntity>(ArgumentType argumentType,
            IEnumerable<MemberInfo> members)
            =>
                $"DECLARE {string.Join(", ", members.WhereDeclaringType<TTriggerEntity>().Select(member => DeclareVariableNameSql(argumentType, member)))}";

        private string CloseCursorSql(string cursorName)
            => $"CLOSE {cursorName}";

        private string DeallocateCursorSql(string cursorName)
            => $"DEALLOCATE {cursorName}";

        private string CloseCursorsBlockSql<TTriggerEntity>(Dictionary<ArgumentType, HashSet<MemberInfo>> members)
        {
            return string.Join(" ", members.Where(x => x.Value.Count > 0)
                .Select(x =>
                    $"{CloseCursorSql(CursorName<TTriggerEntity>(x.Key))} {DeallocateCursorSql(CursorName<TTriggerEntity>(x.Key))}"));
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

        private SqlBuilder DeclareCursorBlocksSql<TTriggerEntity>(
            Dictionary<ArgumentType, HashSet<MemberInfo>> affectedMemberPairs)
        {
            var cursorBlocksSql = affectedMemberPairs
                .Where(x => x.Value.WhereDeclaringType<TTriggerEntity>().Any())
                .Select(x => DeclareCursorBlockSql<TTriggerEntity>(x.Key, x.Value));
            return new SqlBuilder()
                .AppendJoin(" ", cursorBlocksSql.Select(x => x.StringBuilder));
        }

        private SqlBuilder DeclareCursorBlockSql<TTriggerEntity>(ArgumentType argumentType,
            IEnumerable<MemberInfo> affectedMembers)
        {
            var cursorName = CursorName<TTriggerEntity>(argumentType);
            return new SqlBuilder()
                .Append(DeclareCursorSql(cursorName))
                .Append(" ")
                .Append(SelectFromCursorSql<TTriggerEntity>(argumentType, affectedMembers))
                .Append(" ")
                .Append(DeclareCursorVariablesSql<TTriggerEntity>(argumentType, affectedMembers))
                .Append($" OPEN {cursorName}");
        }

        public override SqlBuilder GetTriggerUpsertActionSql<TTriggerEntity, TUpsertEntity>(
            TriggerUpsertAction<TTriggerEntity, TUpsertEntity> triggerUpsertAction)
        {
            var insertStatementSql = GetInsertStatementBodySql(triggerUpsertAction.InsertExpression,
                triggerUpsertAction.InsertExpressionPrefixes);

            var newExpressionArguments = ((NewExpression)triggerUpsertAction.MatchExpression.Body).Arguments
                .Cast<MemberExpression>();

            var newExpressionArgumentPairs = newExpressionArguments.ToDictionary(
                argument => argument,
                argument => GetMemberExpressionSql(argument, triggerUpsertAction.MatchExpressionPrefixes));

            var sqlBuilder = new SqlBuilder(newExpressionArgumentPairs.Select(x => x.Value));

            sqlBuilder
                .Append("SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;")
                .Append($"MERGE {GetTableName(typeof(TUpsertEntity))} USING {GetTableName(typeof(TTriggerEntity))}")
                .Append($" ON ")
                .AppendJoin(" AND ", newExpressionArgumentPairs
                    .Select(memberPair =>
                        $"{GetTableName(typeof(TUpsertEntity))}.{GetColumnName(memberPair.Key.Member)} = {memberPair.Value}"));

            sqlBuilder.Append(" WHEN NOT MATCHED THEN INSERT ")
                .Append(insertStatementSql.StringBuilder);

            if (triggerUpsertAction.OnMatchExpression != null)
            {
                var updateStatementSql = GetUpdateStatementBodySql(triggerUpsertAction.OnMatchExpression,
                    triggerUpsertAction.OnMatchExpressionPrefixes);
                sqlBuilder.MergeColumnsInfo(updateStatementSql);
                sqlBuilder.Append(" WHEN MATCHED THEN UPDATE SET ")
                    .Append(updateStatementSql.StringBuilder);
            }

            return sqlBuilder.Append(";");
        }

        protected override string GetMemberExpressionSql(MemberExpression memberExpression, ArgumentType argumentType)
        {
            return argumentType switch
            {
                ArgumentType.New => VariableNameSql(argumentType, memberExpression.Member),
                ArgumentType.Old => VariableNameSql(argumentType, memberExpression.Member),
                _ => GetColumnName(memberExpression.Member),
            };
        }

        protected override string GetBooleanSqlValue(bool value) => value
            ? "1"
            : "0";

        protected override string GetNewGuidExpressionSql() => "NEWID()";
    }

    internal static class SqlServerProviderExtensions
    {
        public static IEnumerable<MemberInfo> WhereDeclaringType<T>(this IEnumerable<MemberInfo> values)
            => values.Where(x => x.DeclaringType.IsAssignableFrom(typeof(T)));
    }
}