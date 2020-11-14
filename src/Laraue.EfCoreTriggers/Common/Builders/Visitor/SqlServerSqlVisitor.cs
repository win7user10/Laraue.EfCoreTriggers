using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Laraue.EfCoreTriggers.Common.Builders.Visitor
{
    public class SqlServerSqlVisitor : BaseTriggerSqlVisitor
    {
        public SqlServerSqlVisitor(IModel model) : base(model)
        {
        }

        protected override string NewEntityPrefix => "Inserted";

        protected override string OldEntityPrefix => "Deleted";

        public override GeneratedSql GetDropTriggerSql(string triggerName, Type entityType)
            => new GeneratedSql($"DROP TRIGGER {triggerName} ON {GetTableName(entityType)};");

        public override GeneratedSql GetTriggerSql<TTriggerEntity>(Trigger<TTriggerEntity> trigger)
        {
            var actionsSql = trigger.Actions.Select(action => action.BuildSql(this));

            var sqlBuilder = new GeneratedSql(actionsSql);
            sqlBuilder.Append($"CREATE TRIGGER {trigger.Name} ON {GetTableName(typeof(TTriggerEntity))}")
                .Append($" FOR {trigger.TriggerType} AS BEGIN ");

            sqlBuilder.Append(DeclareCursorBlocksSql<TTriggerEntity>(sqlBuilder.AffectedColumns))
                .Append(" ")
                .Append(FetchCursorsSql<TTriggerEntity>(sqlBuilder.AffectedColumns))
                .Append(" WHILE @@FETCH_STATUS = 0")
                .Append(" BEGIN ")
                .AppendJoin(actionsSql.Select(x => x.SqlBuilder))
                .Append(FetchCursorsSql<TTriggerEntity>(sqlBuilder.AffectedColumns))
                .Append(" END ");

            sqlBuilder.Append(CloseCursorsBlockSql(sqlBuilder.AffectedColumns))
                .Append(" END");

            return sqlBuilder;
        }

        public override GeneratedSql GetTriggerActionsSql<TTriggerEntity>(TriggerActions<TTriggerEntity> triggerActions)
        {
            var sqlResult = new GeneratedSql();

            if (triggerActions.ActionConditions.Count > 0)
            {
                var conditionsSql = triggerActions.ActionConditions.Select(actionCondition => actionCondition.BuildSql(this));
                sqlResult.MergeColumnsInfo(conditionsSql);
                sqlResult.Append($"IF (")
                    .AppendJoin(" AND ", conditionsSql.Select(x => x.SqlBuilder))
                    .Append($") ");
            }

            var actionsSql = triggerActions.ActionExpressions.Select(action => action.BuildSql(this));
            sqlResult.MergeColumnsInfo(actionsSql)
                .Append("BEGIN ")
                .AppendJoin("; ", actionsSql.Select(x => x.SqlBuilder))
                .Append(" END ");

            return sqlResult;
        }

        private string CursorName(ArgumentType argumentType)
            => $"{TemporaryTableName(argumentType)}Cursor";

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
            => string.Join(" ", members.Where(x => x.Value.WhereDeclaringType<TTriggerEntity>().Count() > 0).Select(x => FetchCursorSql(x.Key, x.Value)));

        private string FetchCursorSql(ArgumentType argumentType, IEnumerable<MemberInfo> members)
            => $"FETCH NEXT FROM {CursorName(argumentType)} INTO {string.Join(", ", members.Select(member => VariableNameSql(argumentType, member)))}";

        private string SelectFromCursorSql<TTriggerEntity>(ArgumentType argumentType, IEnumerable<MemberInfo> members)
            => $"SELECT {string.Join(", ", members.WhereDeclaringType<TTriggerEntity>().Select(x => GetColumnName(x)))} FROM {TemporaryTableName(argumentType)}";

        private string DeclareCursorVariablesSql<TTriggerEntity>(ArgumentType argumentType, IEnumerable<MemberInfo> members)
            => $"DECLARE {string.Join(", ", members.WhereDeclaringType<TTriggerEntity>().Select(member => DeclareVariableNameSql(argumentType, member)))}";

        private string CloseCursorSql(string cursorName)
            => $"CLOSE {cursorName}";

        private string DeallocateCursorSql(string cursorName)
            => $"DEALLOCATE {cursorName}";

        private string CloseCursorsBlockSql(Dictionary<ArgumentType, HashSet<MemberInfo>> members)
        {
            return string.Join(" ", members.Where(x => x.Value.Count > 0)
                .Select(x => $"{CloseCursorSql(CursorName(x.Key))} {DeallocateCursorSql(CursorName(x.Key))}"));
        }

        private string VariableNameSql(ArgumentType argumentType, MemberInfo member)
        {
            var argumentTypeString = argumentType switch
            {
                ArgumentType.New => "New",
                ArgumentType.Old => "Old",
                _ => throw new InvalidOperationException($"Invalid attempt to generate declaring variable SQL using argument prefix {argumentType}")
            };

            return $"@{argumentTypeString}{member.Name}";
        }

        private string DeclareVariableNameSql(ArgumentType argumentType, MemberInfo member)
            => $"{VariableNameSql(argumentType, member)} {GetSqlServerType((PropertyInfo)member)}";

        private string GetSqlServerType(PropertyInfo propertyInfo)
        {
            var mapping = new Dictionary<Type, string>
            {
                [typeof(bool)] = "bit",
                [typeof(Guid)] = "uniqueidentifier",
                [typeof(decimal)] = "decimal",
                [typeof(int)] = "integer",
                [typeof(string)] = "text",
            };

            if (mapping.TryGetValue(propertyInfo.PropertyType, out var type))
                return type;
            throw new NotSupportedException($"Unknown data type {propertyInfo.PropertyType}");
        }

        private GeneratedSql DeclareCursorBlocksSql<TTriggerEntity>(Dictionary<ArgumentType, HashSet<MemberInfo>> affectedMemberPairs)
        {
            var cursorBlocksSql = affectedMemberPairs
                .Where(x => x.Value.WhereDeclaringType<TTriggerEntity>().Count() > 0)
                .Select(x => DeclareCursorBlockSql<TTriggerEntity>(x.Key, x.Value));
            return new GeneratedSql()
                .AppendJoin(" ", cursorBlocksSql.Select(x => x.SqlBuilder));
        }

        private GeneratedSql DeclareCursorBlockSql<TTriggerEntity>(ArgumentType argumentType, IEnumerable<MemberInfo> affectedMembers)
        {
            var cursorName = CursorName(argumentType);
            return new GeneratedSql()
                .Append(DeclareCursorSql(cursorName))
                .Append(" ")
                .Append(SelectFromCursorSql<TTriggerEntity>(argumentType, affectedMembers))
                .Append(" ")
                .Append(DeclareCursorVariablesSql<TTriggerEntity>(argumentType, affectedMembers))
                .Append($" OPEN {cursorName}");
        }

        private GeneratedSql MatchExpressionSql(NewExpression newExpression)
        {
            var expressionMembers = newExpression.Arguments
                .Select(x => ((MemberExpression)x).Member);

            return new GeneratedSql(new Dictionary<ArgumentType, HashSet<MemberInfo>>
            {
                [ArgumentType.New] = expressionMembers.ToHashSet(),
                [ArgumentType.Old] = expressionMembers.ToHashSet(),
            })
                .Append(string.Join(" AND ", expressionMembers.Select(x => $"{VariableNameSql(ArgumentType.Old, x)} = {VariableNameSql(ArgumentType.New, x)}")));
        }

        public override GeneratedSql GetTriggerUpsertActionSql<TTriggerEntity, TUpsertEntity>(TriggerUpsertAction<TTriggerEntity, TUpsertEntity> triggerUpsertAction)
        {
            var insertStatementSql = GetInsertStatementBodySql(triggerUpsertAction.InsertExpression, triggerUpsertAction.InsertExpressionPrefixes);
            var matchStatementSql = MatchExpressionSql((NewExpression)triggerUpsertAction.MatchExpression.Body);

            var sqlBuilder = new GeneratedSql(new[] { insertStatementSql, matchStatementSql });

            sqlBuilder
                .Append($"IF ({matchStatementSql}) BEGIN INSERT INTO ")
                .Append(insertStatementSql.SqlBuilder)
                .Append(" END");

            if (triggerUpsertAction.OnMatchExpression != null)
            {
                var updateStatementSql = GetUpdateStatementBodySql(triggerUpsertAction.OnMatchExpression, triggerUpsertAction.OnMatchExpressionPrefixes);
                sqlBuilder.MergeColumnsInfo(updateStatementSql);
                sqlBuilder.Append(" ELSE BEGIN UPDATE SET ")
                    .Append(updateStatementSql.SqlBuilder)
                    .Append(" END");
            }

            return sqlBuilder;
        }

        protected override string GetMemberExpressionSql(MemberExpression memberExpression, ArgumentType argumentType)
        {
            return argumentType switch
            {
                ArgumentType.New => VariableNameSql(argumentType, memberExpression.Member),
                ArgumentType.Old => VariableNameSql(argumentType, memberExpression.Member),
                ArgumentType.None => $"{GetTableName(memberExpression.Member)}.{GetColumnName(memberExpression.Member)}",
                _ => GetColumnName(memberExpression.Member),
            };
        }
    }

    internal static class Extensions
    {
        public static IEnumerable<MemberInfo> WhereDeclaringType<T>(this IEnumerable<MemberInfo> values)
            => values.Where(x => x.DeclaringType == typeof(T));

        public static IEnumerable<MemberInfo> FilterByType<T>(this IEnumerable<MemberInfo> values)
            => values.Where(x => x.DeclaringType == typeof(T));
    }
}