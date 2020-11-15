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

        public abstract GeneratedSql GetTriggerSql<TTriggerEntity>(Trigger<TTriggerEntity> trigger)
            where TTriggerEntity : class;

        public abstract GeneratedSql GetDropTriggerSql(string triggerName, Type entityType);

        public virtual GeneratedSql GetTriggerConditionSql<TTriggerEntity>(TriggerCondition<TTriggerEntity> triggerCondition)
            where TTriggerEntity : class
        {
            var conditionBody = triggerCondition.Condition.Body;
            if (conditionBody is BinaryExpression binaryExpression)
                return GetBinaryExpressionSql(binaryExpression, triggerCondition.ConditionPrefixes);
            else if (conditionBody is MemberExpression memberExpression)
                return GetUnaryExpressionSql(Expression.IsTrue(memberExpression), triggerCondition.ConditionPrefixes);
            else if (conditionBody is UnaryExpression unaryExpression)
                return GetUnaryExpressionSql(unaryExpression, triggerCondition.ConditionPrefixes);
            throw new NotImplementedException($"Trigger condition of type {conditionBody.GetType()} is not supported.");
        }

        public abstract GeneratedSql GetTriggerActionsSql<TTriggerEntity>(TriggerActions<TTriggerEntity> triggerActions)
            where TTriggerEntity : class;

        public GeneratedSql GetTriggerUpdateActionSql<TTriggerEntity, TUpdateEntity>(TriggerUpdateAction<TTriggerEntity, TUpdateEntity> triggerUpdateAction)
            where TTriggerEntity : class
            where TUpdateEntity : class
        {
            var updateStatement = GetUpdateStatementBodySql(triggerUpdateAction.UpdateExpression, triggerUpdateAction.UpdateExpressionPrefixes);
            var conditionStatement = GetConditionStatementSql(triggerUpdateAction.UpdateFilter, triggerUpdateAction.UpdateFilterPrefixes);
            return new GeneratedSql(updateStatement.AffectedColumns)
                .MergeColumnsInfo(conditionStatement.AffectedColumns)
                .Append($"UPDATE {GetTableName(typeof(TUpdateEntity))} SET ")
                .Append(updateStatement.SqlBuilder)
                .Append(" ")
                .Append(conditionStatement.SqlBuilder)
                .Append(";");
        }

        public virtual GeneratedSql GetConditionStatementSql(LambdaExpression conditionExpression, Dictionary<string, ArgumentType> argumentTypees)
        {
            var binaryExpressionSql = GetBinaryExpressionSql((BinaryExpression)conditionExpression.Body, argumentTypees);
            return new GeneratedSql(binaryExpressionSql.AffectedColumns)
                .Append("WHERE ")
                .Append(binaryExpressionSql.SqlBuilder);
        }

        public virtual GeneratedSql GetUpdateStatementBodySql(LambdaExpression updateExpression, Dictionary<string, ArgumentType> argumentTypees)
        {
            var assignmentParts = GetMemberInitExpressionAssignmentParts((MemberInitExpression)updateExpression.Body, argumentTypees);
            var sqlResult = new GeneratedSql(assignmentParts.Values);
            sqlResult.Append(string.Join(", ", assignmentParts.Select(expressionPart => $"{GetColumnName(expressionPart.Key)} = {expressionPart.Value}")));
            return sqlResult;
        }

        public virtual GeneratedSql GetInsertStatementBodySql(LambdaExpression insertExpression, Dictionary<string, ArgumentType> argumentTypees)
        {
            var assignmentParts = GetMemberInitExpressionAssignmentParts((MemberInitExpression)insertExpression.Body, argumentTypees);
            var sqlResult = new GeneratedSql(assignmentParts.Values);
            sqlResult.Append($"({string.Join(", ", assignmentParts.Select(x => GetColumnName(x.Key)))})")
                .Append($" VALUES ({string.Join(", ", assignmentParts.Select(x => x.Value))})");
            return sqlResult;
        }

        public abstract GeneratedSql GetTriggerUpsertActionSql<TTriggerEntity, TUpsertEntity>(TriggerUpsertAction<TTriggerEntity, TUpsertEntity> triggerUpsertAction)
            where TTriggerEntity : class
            where TUpsertEntity : class;

        public GeneratedSql GetTriggerDeleteActionSql<TTriggerEntity, TUpdateEntity>(TriggerDeleteAction<TTriggerEntity, TUpdateEntity> triggerDeleteAction)
            where TTriggerEntity : class
            where TUpdateEntity : class
        {
            var conditionStatement = GetConditionStatementSql(triggerDeleteAction.DeleteFilter, triggerDeleteAction.DeleteFilterPrefixes);
            return new GeneratedSql(conditionStatement.AffectedColumns)
                .Append($"DELETE FROM {GetTableName(typeof(TUpdateEntity))} ")
                .Append(conditionStatement.SqlBuilder)
                .Append(";");
        }

        public GeneratedSql GetTriggerInsertActionSql<TTriggerEntity, TInsertEntity>(TriggerInsertAction<TTriggerEntity, TInsertEntity> triggerInsertAction)
            where TTriggerEntity : class
            where TInsertEntity : class
        {
            var insertStatement = GetInsertStatementBodySql(triggerInsertAction.InsertExpression, triggerInsertAction.InsertExpressionPrefixes);
            return new GeneratedSql(insertStatement.AffectedColumns)
                .Append($"INSERT INTO {GetTableName(typeof(TInsertEntity))} ")
                .Append(insertStatement.SqlBuilder)
                .Append(";");
        }
    }
}
