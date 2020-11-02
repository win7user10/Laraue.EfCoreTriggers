using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Laraue.EfCoreTriggers.Common.Builders.Visitor;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
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

        public override string GetDropTriggerSql(string triggerName, Type entityType)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder
                .Append($"DROP TRIGGER {triggerName} ON {GetTableName(entityType)};")
                .Append($"DROP FUNCTION {triggerName}();");
            return sqlBuilder.ToString();
        }

        public override string GetTriggerActionsSql(TriggerActions triggerActions)
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

            sqlBuilder.Append($"RETURN NEW;");

            return sqlBuilder.ToString();
        }

        public override string GetTriggerSql<TTriggerEntity>(Trigger<TTriggerEntity> trigger)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append($"CREATE FUNCTION {trigger.Name}() RETURNS trigger as ${trigger.Name}$ ")
                .Append("BEGIN ");

            foreach (var action in trigger.Actions)
                sqlBuilder.Append(action.BuildSql(this));

            sqlBuilder.Append(" END;")
                .Append($"${trigger.Name}$ LANGUAGE plpgsql;")
                .Append($"CREATE TRIGGER {trigger.Name} {trigger.TriggerTime.ToString().ToUpper()} {trigger.TriggerType.ToString().ToUpper()} ")
                .Append($"ON {GetTableName(typeof(TTriggerEntity))} FOR EACH ROW EXECUTE PROCEDURE {trigger.Name}();");

            return sqlBuilder.ToString();
        }
    }
}
