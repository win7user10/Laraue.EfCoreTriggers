using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
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

        public abstract string GetTriggerSql<TTriggerEntity>(Trigger<TTriggerEntity> trigger)
            where TTriggerEntity : class;

        public abstract string GetDropTriggerSql(string triggerName, Type entityType);

        public virtual string GetTriggerConditionSql<TTriggerEntity>(TriggerCondition<TTriggerEntity> triggerCondition)
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

        public abstract string GetTriggerActionsSql<TTriggerEntity>(TriggerActions<TTriggerEntity> triggerActions)
            where TTriggerEntity : class;

        public string GetTriggerUpdateActionSql<TTriggerEntity, TUpdateEntity>(TriggerUpdateAction<TTriggerEntity, TUpdateEntity> triggerUpdateAction)
            where TTriggerEntity : class
            where TUpdateEntity : class
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder
                .Append($"UPDATE {GetTableName(typeof(TUpdateEntity))} SET ")
                .Append(GetUpdateStatementBodySql(triggerUpdateAction.UpdateExpression, triggerUpdateAction.UpdateExpressionPrefixes))
                .Append(" ")
                .Append(GetConditionStatementSql(triggerUpdateAction.UpdateFilter, triggerUpdateAction.UpdateFilterPrefixes));
            return sqlBuilder.ToString();
        }

        public virtual string GetConditionStatementSql(LambdaExpression conditionExpression, Dictionary<string, ArgumentPrefix> argumentPrefixes)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("WHERE ")
                .Append(GetBinaryExpressionSql((BinaryExpression)conditionExpression.Body, argumentPrefixes));
            return sqlBuilder.ToString();
        }

        public virtual string GetUpdateStatementBodySql(LambdaExpression updateExpression, Dictionary<string, ArgumentPrefix> argumentPrefixes)
        {
            var sqlBuilder = new StringBuilder();
            var assignmentParts = GetMemberInitExpressionAssignmentParts((MemberInitExpression)updateExpression.Body, argumentPrefixes);
            sqlBuilder.Append(string.Join(", ", assignmentParts.Select(expressionPart => $"{expressionPart.Key} = {expressionPart.Value}")));
            return sqlBuilder.ToString();
        }

        public virtual string GetInsertStatementBodySql(LambdaExpression insertExpression, Dictionary<string, ArgumentPrefix> argumentPrefixes)
        {
            var sqlBuilder = new StringBuilder();
            var assignmentParts = GetMemberInitExpressionAssignmentParts((MemberInitExpression)insertExpression.Body, argumentPrefixes);
            sqlBuilder.Append($"({string.Join(", ", assignmentParts.Select(x => x.Key))})")
                .Append($" VALUES ({string.Join(", ", assignmentParts.Select(x => x.Value))})");
            return sqlBuilder.ToString();
        }

        public abstract string GetTriggerUpsertActionSql<TTriggerEntity, TUpsertEntity>(TriggerUpsertAction<TTriggerEntity, TUpsertEntity> triggerUpsertAction)
            where TTriggerEntity : class
            where TUpsertEntity : class;

        public string GetTriggerDeleteActionSql<TTriggerEntity, TUpdateEntity>(TriggerDeleteAction<TTriggerEntity, TUpdateEntity> triggerDeleteAction)
            where TTriggerEntity : class
            where TUpdateEntity : class
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder
                .Append($"DELETE FROM {GetTableName(typeof(TUpdateEntity))} ")
                .Append(GetConditionStatementSql(triggerDeleteAction.DeleteFilter, triggerDeleteAction.DeleteFilterPrefixes));
            return sqlBuilder.ToString();
        }

        public string GetTriggerInsertActionSql<TTriggerEntity, TInsertEntity>(TriggerInsertAction<TTriggerEntity, TInsertEntity> triggerInsertAction)
            where TTriggerEntity : class
            where TInsertEntity : class
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append($"INSERT INTO {GetTableName(typeof(TInsertEntity))} ")
                .Append(GetInsertStatementBodySql(triggerInsertAction.InsertExpression, triggerInsertAction.InsertExpressionPrefixes));
            return sqlBuilder.ToString();
        }
    }
}
