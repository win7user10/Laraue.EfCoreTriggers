using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Laraue.EfCoreTriggers.Common.Builders.Triggers.OnUpdate;
using Laraue.EfCoreTriggers.Common.Builders.Triggers.OnInsert;
using Laraue.EfCoreTriggers.Common.Builders.Triggers.OnDelete;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Laraue.EfCoreTriggers.Common.Builders.Visitor
{
    public abstract class BaseTriggerSqlVisitor : BaseExpressionSqlVisitor, ITriggerSqlVisitor
    {
        public BaseTriggerSqlVisitor(IModel model) : base(model)
        {
        }

        #region Triggers

        public virtual string GetTriggerSql<TTriggerEntity>(OnUpdateTrigger<TTriggerEntity> trigger) where TTriggerEntity : class
            => GetTriggerSql((Trigger<TTriggerEntity>)trigger);

        public virtual string GetTriggerSql<TTriggerEntity>(OnInsertTrigger<TTriggerEntity> trigger) where TTriggerEntity : class
            => GetTriggerSql((Trigger<TTriggerEntity>)trigger);

        public virtual string GetTriggerSql<TTriggerEntity>(OnDeleteTrigger<TTriggerEntity> trigger) where TTriggerEntity : class
            => GetTriggerSql((Trigger<TTriggerEntity>)trigger);

        public abstract string GetTriggerSql<TTriggerEntity>(Trigger<TTriggerEntity> trigger) where TTriggerEntity : class;

        public abstract string GetDropTriggerSql(string triggerName, Type entityType);

        #endregion

        #region TriggerConditions

        public virtual string GetTriggerConditionSql<TTriggerEntity>(OnUpdateTriggerCondition<TTriggerEntity> triggerCondition) where TTriggerEntity : class
            => GetTriggerConditionSqlInternal(triggerCondition, new Dictionary<int, ArgumentPrefix>
            {
                [0] = ArgumentPrefix.Old,
                [1] = ArgumentPrefix.New,
            });

        public virtual string GetTriggerConditionSql<TTriggerEntity>(OnDeleteTriggerCondition<TTriggerEntity> triggerCondition) where TTriggerEntity : class
            => GetTriggerConditionSqlInternal(triggerCondition, new Dictionary<int, ArgumentPrefix>
            {
                [0] = ArgumentPrefix.Old,
            });

        public virtual string GetTriggerConditionSql<TTriggerEntity>(OnInsertTriggerCondition<TTriggerEntity> triggerCondition) where TTriggerEntity : class
            => GetTriggerConditionSqlInternal(triggerCondition, new Dictionary<int, ArgumentPrefix>
            {
                [0] = ArgumentPrefix.New,
            });

        private string GetTriggerConditionSqlInternal(TriggerCondition triggerCondition, Dictionary<int, ArgumentPrefix> argumentPrefixes)
        {
            var conditionLambda = (LambdaExpression)triggerCondition.Condition;
            var expressionArgs = argumentPrefixes.ToDictionary(x => conditionLambda.Parameters[x.Key].Name, x => x.Value);
            return GetTriggerConditionSql(conditionLambda, expressionArgs);
        }

        public virtual string GetTriggerConditionSql(LambdaExpression triggerCondition, Dictionary<string, ArgumentPrefix> argumentPrefixes)
        {
            if (triggerCondition.Body is BinaryExpression binaryExpression)
                return GetBinaryExpressionSql(binaryExpression, argumentPrefixes);
            else if (triggerCondition.Body is MemberExpression memberExpression)
                return GetUnaryExpressionSql(Expression.IsTrue(memberExpression), argumentPrefixes);
            throw new NotImplementedException($"Trigger condition of type {triggerCondition.Body.GetType()} is not supported.");
        }

        #endregion

        #region TriggerActions

        public virtual string GetTriggerActionsSql<TTriggerEntity>(OnUpdateTriggerActions<TTriggerEntity> triggerActions) where TTriggerEntity : class
            => GetTriggerActionsSql((TriggerActions)triggerActions);

        public virtual string GetTriggerActionsSql<TTriggerEntity>(OnDeleteTriggerActions<TTriggerEntity> triggerActions) where TTriggerEntity : class
            => GetTriggerActionsSql((TriggerActions)triggerActions);

        public virtual string GetTriggerActionsSql<TTriggerEntity>(OnInsertTriggerActions<TTriggerEntity> triggerActions) where TTriggerEntity : class
            => GetTriggerActionsSql((TriggerActions)triggerActions);

        public abstract string GetTriggerActionsSql(TriggerActions triggerActions);

        #endregion

        #region TriggerUpdateAction

        public virtual string GetTriggerUpdateActionSql<TTriggerEntity, TUpdateEntity>(OnUpdateTriggerUpdateAction<TTriggerEntity, TUpdateEntity> triggerUpdateAction)
            where TTriggerEntity : class
            where TUpdateEntity : class
                => GetTriggerUpdateActionSqlInternal<TTriggerEntity>(triggerUpdateAction, new Dictionary<int, ArgumentPrefix>
                {
                    [0] = ArgumentPrefix.Old,
                    [1] = ArgumentPrefix.New,
                });

        public virtual string GetTriggerUpdateActionSql<TTriggerEntity, TUpdateEntity>(OnDeleteTriggerUpdateAction<TTriggerEntity, TUpdateEntity> triggerUpdateAction)
            where TTriggerEntity : class
            where TUpdateEntity : class
                => GetTriggerUpdateActionSqlInternal<TTriggerEntity>(triggerUpdateAction, new Dictionary<int, ArgumentPrefix>
                {
                    [0] = ArgumentPrefix.Old,
                });

        public virtual string GetTriggerUpdateActionSql<TTriggerEntity, TUpdateEntity>(OnInsertTriggerUpdateAction<TTriggerEntity, TUpdateEntity> triggerUpdateAction)
            where TTriggerEntity : class
            where TUpdateEntity : class
                => GetTriggerUpdateActionSqlInternal<TTriggerEntity>(triggerUpdateAction, new Dictionary<int, ArgumentPrefix>
                {
                    [0] = ArgumentPrefix.New,
                });

        private string GetTriggerUpdateActionSqlInternal<TUpdateEntity>(TriggerUpdateAction triggerUpdateAction, Dictionary<int, ArgumentPrefix> argumentPrefixes)
            where TUpdateEntity : class
        {
            var setLambda = (LambdaExpression)triggerUpdateAction.UpdateExpression;
            var setCondition = (LambdaExpression)triggerUpdateAction.UpdateFilter;
            var setArgs = argumentPrefixes.ToDictionary(x => setLambda.Parameters[x.Key].Name, x => x.Value);
            var conditionArgs = argumentPrefixes.ToDictionary(x => setCondition.Parameters[x.Key].Name, x => x.Value);
            return GetTriggerUpdateActionSql<TUpdateEntity>(setCondition, conditionArgs, setLambda, setArgs);
        }

        public virtual string GetTriggerUpdateActionSql<TUpdateEntity>(
            LambdaExpression condition, Dictionary<string, ArgumentPrefix> conditionArgumentPrefixes,
            LambdaExpression setExpression, Dictionary<string, ArgumentPrefix> setArgumentPrefixes) where TUpdateEntity : class
        {
            var sqlBuilder = new StringBuilder();

            sqlBuilder.Append("update ")
                .Append($"{GetTableName(typeof(TUpdateEntity))} ");

            sqlBuilder.Append(GetMemberInitSql((MemberInitExpression)setExpression.Body, conditionArgumentPrefixes))
                .Append(" where ")
                .Append(GetBinaryExpressionSql((BinaryExpression)condition.Body, setArgumentPrefixes));

            return sqlBuilder.ToString();
        }

        #endregion
    }
}
