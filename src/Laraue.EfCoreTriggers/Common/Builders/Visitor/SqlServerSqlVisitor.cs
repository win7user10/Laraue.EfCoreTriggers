using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
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

            sqlBuilder.Append(DeclareCursorBlocksSql(trigger.TriggerType, sqlBuilder.AffectedColumns))
                .Append(" ")
                .Append(FetchCursorsSql(sqlBuilder.AffectedColumns))
                .Append(" WHILE @@FETCH_STATUS = 0")
                .Append(" BEGIN ")
                .AppendJoin(actionsSql.Select(x => x.SqlBuilder))
                .Append(FetchCursorsSql(sqlBuilder.AffectedColumns))
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

        private string CursorName(ArgumentPrefix argumentPrefix)
        {
            return argumentPrefix switch
            {
                ArgumentPrefix.New => "InsertedCursor",
                ArgumentPrefix.Old => "DeletedCursor",
                _ => throw new InvalidOperationException($"Cursor name for prefix {argumentPrefix} is not exists")
            };
        }

        private string DeclareCursorSql(string cursorName)
            => $"DECLARE {cursorName} CURSOR FOR";

        private string FetchCursorsSql(Dictionary<ArgumentPrefix, HashSet<MemberInfo>> members)
            => string.Join(" ", members.Where(x => x.Value.Count > 0).Select(x => FetchCursorSql(x.Key, x.Value)));

        private string FetchCursorSql(ArgumentPrefix argumentPrefix, IEnumerable<MemberInfo> members)
            => $"FETCH NEXT FROM {CursorName(argumentPrefix)} INTO {string.Join(", ", members.Select(member => VariableNameSql(argumentPrefix, member)))}";

        private string SelectFromCursorSql(ArgumentPrefix argumentPrefix, IEnumerable<MemberInfo> members)
            => $"SELECT {string.Join(", ", members.Select(x => GetColumnName(x)))} FROM {CursorName(argumentPrefix)}";

        private string DeclareCursorVariablesSql(ArgumentPrefix argumentPrefix, IEnumerable<MemberInfo> members)
            => $"DECLARE {string.Join(", ", members.Select(member => DeclareVariableNameSql(argumentPrefix, member)))}";

        private string CloseCursorSql(string cursorName)
            => $"CLOSE {cursorName}";

        private string DeallocateCursorSql(string cursorName)
            => $"DEALLOCATE {cursorName}";

        private string CloseCursorsBlockSql(Dictionary<ArgumentPrefix, HashSet<MemberInfo>> members)
        {
            return string.Join(" ", members.Where(x => x.Value.Count > 0)
                .Select(x => $"{CloseCursorSql(CursorName(x.Key))} {DeallocateCursorSql(CursorName(x.Key))}"));
        }

        private string VariableNameSql(ArgumentPrefix argumentPrefix, MemberInfo member)
        {
            var argumentPrefixString = argumentPrefix switch
            {
                ArgumentPrefix.New => "New",
                ArgumentPrefix.Old => "Old",
                _ => throw new InvalidOperationException($"Invalid attempt to generate declaring variable SQL using argument prefix {argumentPrefix}")
            };

            return $"@{argumentPrefixString}{member.Name}";
        }

        private string DeclareVariableNameSql(ArgumentPrefix argumentPrefix, MemberInfo member)
            => $"{VariableNameSql(argumentPrefix, member)} {GetSqlServerType((PropertyInfo)member)}";

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

        private GeneratedSql DeclareCursorBlocksSql(TriggerType triggerType, Dictionary<ArgumentPrefix, HashSet<MemberInfo>> affectedMemberPairs)
        {
            var cursorBlocksSql = affectedMemberPairs
                .Where(x => x.Value.Count > 0)
                .Select(x => DeclareCursorBlockSql(triggerType, x.Key, x.Value));
            return new GeneratedSql()
                .AppendJoin(cursorBlocksSql.Select(x => x.SqlBuilder));
        }

        private GeneratedSql DeclareCursorBlockSql(TriggerType triggerType, ArgumentPrefix argumentPrefix, IEnumerable<MemberInfo> affectedMembers)
        {
            var cursorName = CursorName(argumentPrefix);
            return new GeneratedSql()
                .Append(DeclareCursorSql(cursorName))
                .Append(" ")
                .Append(SelectFromCursorSql(argumentPrefix, affectedMembers))
                .Append(" ")
                .Append(DeclareCursorVariablesSql(argumentPrefix, affectedMembers))
                .Append($" OPEN {cursorName}");
        }

        public override GeneratedSql GetTriggerUpsertActionSql<TTriggerEntity, TUpsertEntity>(TriggerUpsertAction<TTriggerEntity, TUpsertEntity> triggerUpsertAction)
        {
            return new GeneratedSql();
            throw new NotImplementedException();
        }
    }
}