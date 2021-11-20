using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Laraue.EfCoreTriggers.Common.SqlGeneration
{
    public abstract class BaseSqlProvider : BaseExpressionProvider, ITriggerProvider
    {
        protected BaseSqlProvider(IReadOnlyModel model) : base(model)
        {
        }

        protected virtual Dictionary<TriggerTime, string> TriggerTimeNames { get; } = new()
        {
            [TriggerTime.After] = "AFTER",
            [TriggerTime.Before] = "BEFORE",
            [TriggerTime.InsteadOf] = "INSTEAD OF",
        };

        protected virtual IEnumerable<TriggerTime> AvailableTriggerTimes { get; } = new[]
        {
            TriggerTime.After,
            TriggerTime.Before, 
            TriggerTime.InsteadOf
        };

        protected string GetTriggerTimeName(TriggerTime triggerTime)
        {
            if (!AvailableTriggerTimes.Contains(triggerTime) ||
                !TriggerTimeNames.TryGetValue(triggerTime, out var triggerTypeName))
            {
                throw new NotSupportedException($"Trigger time {triggerTime} is not supported for {GetType()}.");
            }

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

        public virtual SqlBuilder GetUpdateStatementBodySql(LambdaExpression updateExpression, Dictionary<string, ArgumentType> argumentTypes)
        {
            var assignmentParts = GetLambdaNewExpressionParts(updateExpression, argumentTypes);
            var sqlResult = new SqlBuilder(assignmentParts.Values);
            var assignmentPartsSql = assignmentParts
                .Select(expressionPart => $"{GetColumnName(expressionPart.Key)} = {expressionPart.Value}")
                .ToArray();
            sqlResult.AppendJoin(", ", assignmentPartsSql);
            return sqlResult;
        }

        public virtual SqlBuilder GetInsertStatementBodySql(LambdaExpression insertExpression, Dictionary<string, ArgumentType> argumentTypes)
        {
            var assignmentParts = GetLambdaNewExpressionParts(insertExpression, argumentTypes);
            var sqlResult = new SqlBuilder(assignmentParts.Values);

            if (assignmentParts.Any())
            {
                sqlResult.Append("(")
                    .AppendJoin(", ", assignmentParts.Select(x => $"{Delimiter}{GetColumnName(x.Key)}{Delimiter}"))
                    .Append(") VALUES (")
                    .AppendJoin(", ", assignmentParts.Select(x => x.Value.ToString()))
                    .Append(")");
            }
            else
            {
                sqlResult.Append(GetEmptyInsertStatementBodySql(insertExpression, argumentTypes));
            }
            
            return sqlResult;
        }


        protected virtual SqlBuilder GetEmptyInsertStatementBodySql(LambdaExpression insertExpression,
            Dictionary<string, ArgumentType> argumentTypes)
        {
            var sqlBuilder = new SqlBuilder();
            sqlBuilder.Append("DEFAULT VALUES");
            return sqlBuilder;
        }

        public virtual SqlBuilder GetTriggerUpsertActionSql<TTriggerEntity, TUpdateEntity>(TriggerUpsertAction<TTriggerEntity, TUpdateEntity> triggerUpsertAction)
            where TTriggerEntity : class
            where TUpdateEntity : class
        {
            var matchExpressionParts = GetLambdaNewExpressionParts(triggerUpsertAction.MatchExpression, triggerUpsertAction.MatchExpressionPrefixes);
            var insertStatementSql = GetInsertStatementBodySql(triggerUpsertAction.InsertExpression, triggerUpsertAction.InsertExpressionPrefixes);
            
            var sqlBuilder = new SqlBuilder(insertStatementSql.AffectedColumns)
                .MergeColumnsInfo(matchExpressionParts.Values)
                .Append($"INSERT INTO {GetTableName(typeof(TUpdateEntity))} ")
                .Append(insertStatementSql.StringBuilder)
                .Append(" ON CONFLICT (")
                .AppendJoin(", ", matchExpressionParts.Select(x => GetColumnName(x.Key)))
                .Append(")");

            if (triggerUpsertAction.OnMatchExpression is null)
            {
                sqlBuilder.Append(" DO NOTHING;");
            }
            else
            {
                var updateStatementSql = GetUpdateStatementBodySql(triggerUpsertAction.OnMatchExpression, triggerUpsertAction.OnMatchExpressionPrefixes);
                sqlBuilder.MergeColumnsInfo(updateStatementSql.AffectedColumns)
                    .Append(" DO UPDATE SET ")
                    .Append(updateStatementSql.StringBuilder)
                    .Append(";");
            }

            return sqlBuilder;
        }
        
        protected Dictionary<MemberInfo, SqlBuilder> GetLambdaNewExpressionParts(LambdaExpression expression, Dictionary<string, ArgumentType> argumentTypes)
        {
            return expression.Body switch
            {
                MemberInitExpression memberInitExpression => GetMemberInitExpressionAssignmentParts(memberInitExpression, argumentTypes)
                    .ToDictionary(x => x.Key.Member, x => x.Value),
                NewExpression newExpression => GetNewExpressionAssignmentParts(newExpression, argumentTypes)
                    .ToDictionary(x => x.Key.Member, x => x.Value),
                _ => throw new NotImplementedException()
            };
        }

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
        
        public SqlBuilder GetTriggerRawActionSql<TTriggerEntity>(TriggerRawAction<TTriggerEntity> triggerRawAction)
            where TTriggerEntity : class
        {
            var sqlBuilder = new SqlBuilder();

            if (triggerRawAction.ArgumentSelectorExpressions.Length > 0)
            {
                var sqlArgBuilders = new List<SqlBuilder>();
                
                for (var i = 0; i < triggerRawAction.ArgumentSelectorExpressions.Length; i++)
                {
                    var expression = triggerRawAction.ArgumentSelectorExpressions[i];
                    var prefixes = triggerRawAction.ArgumentPrefixes[i];

                    var argSql = GetExpressionSql(expression.Body, prefixes);
                    
                    sqlArgBuilders.Add(argSql);
                    sqlBuilder.MergeColumnsInfo(argSql);
                }

                sqlBuilder.Append(string.Format(triggerRawAction.Sql, sqlArgBuilders.ToArray()));
            }
            else
            {
                sqlBuilder.Append(triggerRawAction.Sql);
            }

            return sqlBuilder;
        }
    }
}
