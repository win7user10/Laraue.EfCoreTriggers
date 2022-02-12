﻿using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;
using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors.Statements;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.MySql;

public class MySqlTriggerUpsertActionVisitor : ITriggerActionVisitor<TriggerUpsertAction>
{
    private readonly IInsertExpressionVisitor _insertExpressionVisitor;
    private readonly IUpdateExpressionVisitor _updateExpressionVisitor;
    private readonly IDbSchemaRetriever _adapter;

    public MySqlTriggerUpsertActionVisitor(
        IInsertExpressionVisitor insertExpressionVisitor,
        IUpdateExpressionVisitor updateExpressionVisitor,
        IDbSchemaRetriever adapter)
    {
        _insertExpressionVisitor = insertExpressionVisitor;
        _updateExpressionVisitor = updateExpressionVisitor;
        _adapter = adapter;
    }
    
    public SqlBuilder Visit(TriggerUpsertAction triggerAction, VisitedMembers visitedMembers)
    {
        var updateEntityType = triggerAction.InsertExpression.Body.Type;

        var insertStatementSql = _insertExpressionVisitor.Visit(
            triggerAction.InsertExpression,
            triggerAction.InsertExpressionPrefixes,
            visitedMembers);

        var sqlBuilder = new SqlBuilder();

        if (triggerAction.OnMatchExpression is null)
        {
            sqlBuilder.Append($"INSERT IGNORE {_adapter.GetTableName(updateEntityType)} ")
                .Append(insertStatementSql)
                .Append(";");
        }
        else
        {
            var updateStatementSql = _updateExpressionVisitor.Visit(
                triggerAction.OnMatchExpression, 
                triggerAction.OnMatchExpressionPrefixes,
                visitedMembers);
            
            sqlBuilder.Append($"INSERT INTO {_adapter.GetTableName(updateEntityType)} ")
                .Append(insertStatementSql)
                .AppendNewLine("ON DUPLICATE KEY")
                .AppendNewLine("UPDATE ")
                .Append(updateStatementSql)
                .Append(";");
        }

        return sqlBuilder;
    }
}