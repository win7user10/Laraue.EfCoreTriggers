using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Visitor
{
    public class SqlServerSqlVisitor : BaseTriggerSqlVisitor
    {
        public SqlServerSqlVisitor(IModel model) : base(model)
        {
        }

        protected override string NewEntityPrefix => throw new NotImplementedException();

        protected override string OldEntityPrefix => throw new NotImplementedException();

        protected override char Quote => throw new NotImplementedException();

        public override string GetDropTriggerSql(string triggerName, Type entityType)
        {
            throw new NotImplementedException();
        }

        public override string GetMethodConcatCallExpressionSql(MethodCallExpression methodCallExpression, params string[] concatExpressionArgsSql)
        {
            throw new NotImplementedException();
        }

        public override string GetMethodToLowerCallExpressionSql(MethodCallExpression methodCallExpression, string lowerSqlExpression)
        {
            throw new NotImplementedException();
        }

        public override string GetMethodToUpperCallExpressionSql(MethodCallExpression methodCallExpression, string upperSqlExpression)
        {
            throw new NotImplementedException();
        }

        public override string GetTriggerActionsSql<TTriggerEntity>(TriggerActions<TTriggerEntity> triggerActions)
        {
            throw new NotImplementedException();
        }

        public override string GetTriggerSql<TTriggerEntity>(Trigger<TTriggerEntity> trigger)
        {
            throw new NotImplementedException();
        }

        public override string GetTriggerUpsertActionSql<TTriggerEntity, TUpsertEntity>(TriggerUpsertAction<TTriggerEntity, TUpsertEntity> triggerUpsertAction)
        {
            throw new NotImplementedException();
        }
    }
}
