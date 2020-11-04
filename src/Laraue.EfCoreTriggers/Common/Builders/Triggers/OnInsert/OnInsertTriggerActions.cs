using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Laraue.EfCoreTriggers.Common.Builders.Visitor;
using System;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.OnInsert
{
    public class OnInsertTriggerActions<TTriggerEntity> : TriggerActions<TTriggerEntity>
        where TTriggerEntity : class
    {
        public OnInsertTriggerActions<TTriggerEntity> Condition(Expression<Func<TTriggerEntity, bool>> condition)
        {
            ActionConditions.Add(new OnInsertTriggerCondition<TTriggerEntity>(condition));
            return this;
        }

        public OnInsertTriggerActions<TTriggerEntity> Update<TUpdateEntity>(
            Expression<Func<TTriggerEntity, TUpdateEntity, bool>> entityFilter,
            Expression<Func<TTriggerEntity, TUpdateEntity, TUpdateEntity>> setValues)
            where TUpdateEntity : class
        {
            ActionExpressions.Add(new OnInsertTriggerUpdateAction<TTriggerEntity, TUpdateEntity>(entityFilter, setValues));
            return this;
        }

        public OnInsertTriggerActions<TTriggerEntity> Upsert<TUpsertEntity>(
            Expression<Func<TUpsertEntity, object>> matchExpression,
            Expression<Func<TTriggerEntity, TUpsertEntity>> insertExpression,
            Expression<Func<TTriggerEntity, TUpsertEntity, TUpsertEntity>> onMatchExpression)
            where TUpsertEntity : class
        {
            ActionExpressions.Add(new OnInsertTriggerUpsertAction<TTriggerEntity, TUpsertEntity>(matchExpression, insertExpression, onMatchExpression));
            return this;
        }

        public override string BuildSql(ITriggerSqlVisitor visitor)
        {
            return visitor.GetTriggerActionsSql(this);
        }
    }
}
