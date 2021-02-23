using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Providers
{
    public abstract class BaseTriggerProvider : BaseExpressionProvider, ITriggerProvider
    {
        public BaseTriggerProvider(IModel model) : base(model)
        {
        }

        protected virtual Dictionary<TriggerTime, string> TriggerTimeNames { get; } = new Dictionary<TriggerTime, string>
        {
            [TriggerTime.After] = "AFTER",
            [TriggerTime.Before] = "BEFORE",
            [TriggerTime.InsteadOf] = "INSTEAD OF",
        };

        protected virtual IEnumerable<TriggerTime> AvailableTriggerTimes { get; } = new[] { TriggerTime.After, TriggerTime.Before, TriggerTime.InsteadOf };

        protected string GetTriggerTimeName(TriggerTime triggerTime)
        {
            if (!AvailableTriggerTimes.Contains(triggerTime) || !TriggerTimeNames.TryGetValue(triggerTime, out var triggerTypeName))
                throw new NotSupportedException($"Trigger time {triggerTime} is not supported for {GetType()}.");
            return triggerTypeName;
        }

        public abstract SqlBuilder GetTriggerSql<TTriggerEntity>(Trigger<TTriggerEntity> trigger)
            where TTriggerEntity : class;

        public abstract SqlBuilder GetDropTriggerSql(string triggerName);

        public virtual SqlBuilder GetTriggerConditionSql<TTriggerEntity>(TriggerCondition<TTriggerEntity> triggerCondition)
            where TTriggerEntity : class
        {
            var conditionBody = triggerCondition.Condition.Body;
            return conditionBody switch
            {
                MemberExpression memberExpression => GetUnaryExpressionSql(Expression.IsTrue(memberExpression), triggerCondition.ConditionPrefixes),
                _ => GetExpressionSql(conditionBody, triggerCondition.ConditionPrefixes),
            };
        }

        public abstract SqlBuilder GetTriggerActionsSql<TTriggerEntity>(TriggerActions<TTriggerEntity> triggerActions)
            where TTriggerEntity : class;

        public SqlBuilder GetTriggerUpdateActionSql<TTriggerEntity, TUpdateEntity>(TriggerUpdateAction<TTriggerEntity, TUpdateEntity> triggerUpdateAction)
            where TTriggerEntity : class
            where TUpdateEntity : class
        {
            var updateStatement = GetUpdateStatementBodySql(triggerUpdateAction.UpdateExpression, triggerUpdateAction.UpdateExpressionPrefixes);
            var conditionStatement = GetConditionStatementSql(triggerUpdateAction.UpdateFilter, triggerUpdateAction.UpdateFilterPrefixes);
            return new SqlBuilder(updateStatement.AffectedColumns)
                .MergeColumnsInfo(conditionStatement.AffectedColumns)
                .Append($"UPDATE {GetTableName(typeof(TUpdateEntity))} SET ")
                .Append(updateStatement.StringBuilder)
                .Append(" ")
                .Append(conditionStatement.StringBuilder)
                .Append(";");
        }

        public virtual SqlBuilder GetConditionStatementSql(LambdaExpression conditionExpression, Dictionary<string, ArgumentType> argumentTypees)
        {
            var binaryExpressionSql = GetBinaryExpressionSql((BinaryExpression)conditionExpression.Body, argumentTypees);
            return new SqlBuilder(binaryExpressionSql.AffectedColumns)
                .Append("WHERE ")
                .Append(binaryExpressionSql.StringBuilder);
        }

        public virtual SqlBuilder GetUpdateStatementBodySql(LambdaExpression updateExpression, Dictionary<string, ArgumentType> argumentTypees)
        {
            var assignmentParts = GetMemberInitExpressionAssignmentParts((MemberInitExpression)updateExpression.Body, argumentTypees);
            var sqlResult = new SqlBuilder(assignmentParts.Values);
            sqlResult.Append(string.Join(", ", assignmentParts.Select(expressionPart => $"{GetColumnName(expressionPart.Key)} = {expressionPart.Value}")));
            return sqlResult;
        }

        public virtual SqlBuilder GetInsertStatementBodySql(LambdaExpression insertExpression, Dictionary<string, ArgumentType> argumentTypees)
        {
            var assignmentParts = GetMemberInitExpressionAssignmentParts((MemberInitExpression)insertExpression.Body, argumentTypees);
            var sqlResult = new SqlBuilder(assignmentParts.Values);
            sqlResult.Append($"({string.Join(", ", assignmentParts.Select(x => GetColumnName(x.Key)))})")
                .Append($" VALUES ({string.Join(", ", assignmentParts.Select(x => x.Value))})");
            return sqlResult;
        }

        public abstract SqlBuilder GetTriggerUpsertActionSql<TTriggerEntity, TUpsertEntity>(TriggerUpsertAction<TTriggerEntity, TUpsertEntity> triggerUpsertAction)
            where TTriggerEntity : class
            where TUpsertEntity : class;

        public SqlBuilder GetTriggerDeleteActionSql<TTriggerEntity, TUpdateEntity>(TriggerDeleteAction<TTriggerEntity, TUpdateEntity> triggerDeleteAction)
            where TTriggerEntity : class
            where TUpdateEntity : class
        {
            var conditionStatement = GetConditionStatementSql(triggerDeleteAction.DeleteFilter, triggerDeleteAction.DeleteFilterPrefixes);
            return new SqlBuilder(conditionStatement.AffectedColumns)
                .Append($"DELETE FROM {GetTableName(typeof(TUpdateEntity))} ")
                .Append(conditionStatement.StringBuilder)
                .Append(";");
        }

        public SqlBuilder GetTriggerInsertActionSql<TTriggerEntity, TInsertEntity>(TriggerInsertAction<TTriggerEntity, TInsertEntity> triggerInsertAction)
            where TTriggerEntity : class
            where TInsertEntity : class
        {
            var insertStatement = GetInsertStatementBodySql(triggerInsertAction.InsertExpression, triggerInsertAction.InsertExpressionPrefixes);
            return new SqlBuilder(insertStatement.AffectedColumns)
                .Append($"INSERT INTO {GetTableName(typeof(TInsertEntity))} ")
                .Append(insertStatement.StringBuilder)
                .Append(";");
        }
    }
}
