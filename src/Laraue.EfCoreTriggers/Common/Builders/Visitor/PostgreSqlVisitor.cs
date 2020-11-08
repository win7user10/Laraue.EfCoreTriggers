using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Laraue.EfCoreTriggers.Common.Builders.Visitor;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq.Expressions;
using System.Text;

namespace Laraue.EfCoreTriggers.Common.Builders.Providers
{
    public class PostgreSqlVisitor : BaseTriggerSqlVisitor
    {
        public PostgreSqlVisitor(IModel model) : base(model)
        {
        }

        protected override string NewEntityPrefix => "NEW";

        protected override string OldEntityPrefix => "OLD";

        protected override char Quote => '\'';

        public override string GetDropTriggerSql(string triggerName, Type entityType)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder
                .Append($"DROP TRIGGER {triggerName} ON {GetTableName(entityType)};")
                .Append($"DROP FUNCTION {triggerName}();");
            return sqlBuilder.ToString();
        }

        public override string GetTriggerActionsSql<TTriggerEntity>(TriggerActions<TTriggerEntity> triggerActions)
        {
            var sqlBuilder = new StringBuilder();
            if (triggerActions.ActionConditions.Count > 0)
            {
                sqlBuilder.Append($"IF ");
                foreach (var actionCondition in triggerActions.ActionConditions)
                {
                    sqlBuilder.Append(actionCondition.BuildSql(this));
                }
                sqlBuilder.Append($" THEN ");
            }

            foreach (var actionExpression in triggerActions.ActionExpressions)
            {
                sqlBuilder.Append(actionExpression.BuildSql(this))
                    .Append(";");
            }

            if (triggerActions.ActionConditions.Count > 0)
                sqlBuilder.Append($"END IF;");

            return sqlBuilder.ToString();
        }

        public override string GetTriggerSql<TTriggerEntity>(Trigger<TTriggerEntity> trigger)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append($"CREATE FUNCTION {trigger.Name}() RETURNS trigger as ${trigger.Name}$ ")
                .Append("BEGIN ");

            foreach (var action in trigger.Actions)
                sqlBuilder.Append(action.BuildSql(this));

            sqlBuilder.Append(" RETURN NEW;END;")
                .Append($"${trigger.Name}$ LANGUAGE plpgsql;")
                .Append($"CREATE TRIGGER {trigger.Name} {trigger.TriggerTime.ToString().ToUpper()} {trigger.TriggerType.ToString().ToUpper()} ")
                .Append($"ON {GetTableName(typeof(TTriggerEntity))} FOR EACH ROW EXECUTE PROCEDURE {trigger.Name}();");

            return sqlBuilder.ToString();
        }

        public override string GetTriggerUpsertActionSql<TTriggerEntity, TUpdateEntity>(TriggerUpsertAction<TTriggerEntity, TUpdateEntity> triggerUpsertAction)
        {
            var sqlBuilder = new StringBuilder();

            sqlBuilder.Append($"INSERT INTO {GetTableName(typeof(TUpdateEntity))} ")
                .Append(GetInsertStatementBodySql(triggerUpsertAction.InsertExpression, triggerUpsertAction.InsertExpressionPrefixes))
                .Append($" ON CONFLICT ({string.Join(", ", GetNewExpressionColumns((NewExpression)triggerUpsertAction.MatchExpression.Body))})");

            if (triggerUpsertAction.OnMatchExpression is null)
                sqlBuilder.Append(" DO NOTHING");
            else
                sqlBuilder.Append($" DO UPDATE SET ")
                .Append(GetUpdateStatementBodySql(triggerUpsertAction.OnMatchExpression, triggerUpsertAction.OnMatchExpressionPrefixes));

            return sqlBuilder.ToString();
        }

        public override string GetMethodConcatCallExpressionSql(MethodCallExpression methodCallExpression, string[] concatExpressionArgsSql)
            => $"CONCAT({string.Join(", ", concatExpressionArgsSql)})";

        public override string GetMethodToLowerCallExpressionSql(MethodCallExpression methodCallExpression, string lowerSqlExpression)
            => $"LOWER({lowerSqlExpression})";

        public override string GetMethodToUpperCallExpressionSql(MethodCallExpression methodCallExpression, string upperSqlExpression)
            => $"UPPER({upperSqlExpression})";
    }
}
