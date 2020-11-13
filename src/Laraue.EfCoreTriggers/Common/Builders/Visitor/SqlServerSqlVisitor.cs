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
            var sqlBuilder = new GeneratedSql();
            sqlBuilder.Append($"CREATE TRIGGER {trigger.Name} ON {GetTableName(typeof(TTriggerEntity))}")
                .Append($" FOR {trigger.TriggerType} AS BEGIN ");

            sqlBuilder.Append(DeclareCursorBlocksSql(trigger.TriggerType, sqlBuilder.AffectedColumns));
            sqlBuilder.Append(FetchCursorsSql(sqlBuilder.AffectedColumns))
                .Append("WHILE @@FETCH_STATUS = 0");

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
            => string.Join(" ", members.Select(x => FetchCursorSql(x.Key, x.Value)));

        private string FetchCursorSql(ArgumentPrefix argumentPrefix, IEnumerable<MemberInfo> members)
            => $"FETCH NEXT FROM {CursorName(argumentPrefix)} INTO {string.Join(", ", members.Select(member => VariableNameSql(argumentPrefix, member)))}";

        private string SelectFromCursorSql(TriggerType triggerType, IEnumerable<MemberInfo> members)
            => $"SELECT {string.Join(", ", members.Select(x => GetColumnName(x)))} FROM {triggerType.ToString().ToUpper()}";

        private string DeclareCursorVariablesSql(ArgumentPrefix argumentPrefix, IEnumerable<MemberInfo> members)
            => $"DECLARE {string.Join(", ", members.Select(member => VariableNameSql(argumentPrefix, member)))}";

        private string VariableNameSql(ArgumentPrefix argumentPrefix, MemberInfo member)
        {
            var argumentPrefixString = argumentPrefix switch
            {
                ArgumentPrefix.New => "New",
                ArgumentPrefix.Old => "Old",
                _ => throw new InvalidOperationException($"Invalid attempt to generate declaring variable SQL using argument prefix {argumentPrefix}")
            };

            return $"@{argumentPrefixString} {GetSqlServerType(member)}";
        }

        private string GetSqlServerType(MemberInfo memberInfo)
            => "int";

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
                .Append("SELECT ")
                .Append(SelectFromCursorSql(triggerType, affectedMembers))
                .Append(DeclareCursorVariablesSql(argumentPrefix, affectedMembers))
                .Append($" OPEN {cursorName}");
        }

        public override GeneratedSql GetTriggerUpsertActionSql<TTriggerEntity, TUpsertEntity>(TriggerUpsertAction<TTriggerEntity, TUpsertEntity> triggerUpsertAction)
        {
            throw new NotImplementedException();
        }
    }
}