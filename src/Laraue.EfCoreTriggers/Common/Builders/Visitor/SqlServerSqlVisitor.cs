using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
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

        private const string DeletedCursorName = "DeletedCursor";

        private const string InsertedCursorName = "InsertedCursor";

        public override GeneratedSql GetDropTriggerSql(string triggerName, Type entityType)
            => new GeneratedSql($"DROP TRIGGER {triggerName} ON {GetTableName(entityType)};");

        public override GeneratedSql GetTriggerSql<TTriggerEntity>(Trigger<TTriggerEntity> trigger)
        {
            var sqlBuilder = new GeneratedSql();
            sqlBuilder.Append($"CREATE TRIGGER {trigger.Name} ON {GetTableName(typeof(TTriggerEntity))}")
                .Append($" FOR {trigger.TriggerType} AS BEGIN ");

            sqlBuilder.Append("DECLARE TempCursor CURSOR FOR");
            // TODO not ready

            return sqlBuilder;
        }

        public override GeneratedSql GetTriggerActionsSql<TTriggerEntity>(TriggerActions<TTriggerEntity> triggerActions)
        {
            throw new NotImplementedException();
        }


        public override GeneratedSql GetTriggerUpsertActionSql<TTriggerEntity, TUpsertEntity>(TriggerUpsertAction<TTriggerEntity, TUpsertEntity> triggerUpsertAction)
        {
            throw new NotImplementedException();
        }
    }
}