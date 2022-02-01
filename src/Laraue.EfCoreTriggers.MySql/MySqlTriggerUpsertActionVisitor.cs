﻿using System.Linq;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Impl.SetExpressionVisitors;
using Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;

namespace Laraue.EfCoreTriggers.MySql;

public class MySqlTriggerUpsertActionVisitor : ITriggerActionVisitor<TriggerUpsertAction>
{
    private readonly ISetExpressionVisitorFactory _factory;
    private readonly TriggerInsertActionVisitor _insertActionVisitor;
    private readonly TriggerUpdateActionVisitor _updateActionVisitor;
    private readonly IEfCoreMetadataRetriever _metadataRetriever;

    public MySqlTriggerUpsertActionVisitor(
        ISetExpressionVisitorFactory factory,
        TriggerInsertActionVisitor insertActionVisitor,
        TriggerUpdateActionVisitor updateActionVisitor,
        IEfCoreMetadataRetriever metadataRetriever)
    {
        _factory = factory;
        _insertActionVisitor = insertActionVisitor;
        _updateActionVisitor = updateActionVisitor;
        _metadataRetriever = metadataRetriever;
    }
    
    public SqlBuilder Visit(TriggerUpsertAction triggerAction, VisitedMembers visitedMembers)
    {
        var matchExpressionParts = _factory.Visit(
            triggerAction.MatchExpression,
            triggerAction.MatchExpressionPrefixes,
            visitedMembers);
        
        var insertStatementSql = _insertActionVisitor.GetInsertStatementSql(
            triggerAction.InsertExpression,
            triggerAction.InsertExpressionPrefixes,
            visitedMembers);

        var updateEntityType = triggerAction.InsertExpression.Parameters[0].Type;

        var sqlBuilder = new SqlBuilder();

        if (triggerAction.OnMatchExpression is null)
        {
            sqlBuilder.Append($"INSERT IGNORE {_metadataRetriever.GetTableName(updateEntityType)} ")
                .Append(insertStatementSql)
                .Append(";");
        }
        else
        {
            var updateStatementSql = _updateActionVisitor.GetUpdateStatementBodySql(
                triggerAction.OnMatchExpression, 
                triggerAction.OnMatchExpressionPrefixes,
                visitedMembers);
            
            sqlBuilder.Append($"INSERT INTO {_metadataRetriever.GetTableName(updateEntityType)} ")
                .Append(insertStatementSql)
                .Append(" ON DUPLICATE KEY UPDATE ")
                .Append(updateStatementSql)
                .Append(";");
        }

        return sqlBuilder;
    }
}