using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Laraue.EfCoreTriggers.Common.Builders.Visitor
{
    public class SqlServerSqlVisitor : BaseTriggerSqlVisitor
    {
        public SqlServerSqlVisitor(IModel model) : base(model)
        {
        }

        protected override string NewEntityPrefix => "Inserted";

        protected override string OldEntityPrefix => "Deleted";

        public override string GetDropTriggerSql(string triggerName, Type entityType)
            => $"DROP TRIGGER {triggerName} ON {GetTableName(entityType)};";

        public override string GetTriggerSql<TTriggerEntity>(Trigger<TTriggerEntity> trigger)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append($"CREATE TRIGGER {trigger.Name} ON {GetTableName(typeof(TTriggerEntity))}")
                .Append($" FOR {trigger.TriggerType} AS BEGIN ");
            foreach (var action in trigger.Actions)
                sqlBuilder.Append(action.BuildSql(this));
            sqlBuilder.Append(" GO");
            return sqlBuilder.ToString();
        }

        public override string GetTriggerActionsSql<TTriggerEntity>(TriggerActions<TTriggerEntity> triggerActions)
        {
            var sqlBuilder = new StringBuilder();
            if (triggerActions.ActionConditions.Count > 0)
            {
                sqlBuilder.Append($"IF ");
                var conditionsSql = triggerActions.ActionConditions.Select(actionCondition => actionCondition.BuildSql(this));
                sqlBuilder.Append(string.Join(" AND ", conditionsSql));
                sqlBuilder.Append($" BEGIN ");
            }

            foreach (var actionExpression in triggerActions.ActionExpressions)
            {
                sqlBuilder.Append(actionExpression.BuildSql(this))
                    .Append(";");
            }

            if (triggerActions.ActionConditions.Count > 0)
                sqlBuilder.Append($"END ");

            return sqlBuilder.ToString();
        }


        public override string GetTriggerUpsertActionSql<TTriggerEntity, TUpsertEntity>(TriggerUpsertAction<TTriggerEntity, TUpsertEntity> triggerUpsertAction)
        {
            throw new NotImplementedException();

            var sqlBuilder = new StringBuilder();

            sqlBuilder.Append($"MERGE {GetTableName(typeof(TUpsertEntity))}");

            sqlBuilder.Append($"INSERT INTO {GetTableName(typeof(TUpsertEntity))} ")
                .Append(GetInsertStatementBodySql(triggerUpsertAction.InsertExpression, triggerUpsertAction.InsertExpressionPrefixes))
                .Append($" ON CONFLICT ({string.Join(", ", GetNewExpressionColumns((NewExpression)triggerUpsertAction.MatchExpression.Body))})");

            if (triggerUpsertAction.OnMatchExpression is null)
                sqlBuilder.Append(" DO NOTHING");
            else
                sqlBuilder.Append($" DO UPDATE SET ")
                .Append(GetUpdateStatementBodySql(triggerUpsertAction.OnMatchExpression, triggerUpsertAction.OnMatchExpressionPrefixes));

            return sqlBuilder.ToString();
        }
    }
}