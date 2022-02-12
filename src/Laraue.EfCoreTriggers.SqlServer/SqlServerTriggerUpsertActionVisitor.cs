﻿using System.Linq;
using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.Services.Impl.SetExpressionVisitors;
using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;
using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors.Statements;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.SqlServer;

public class SqlServerTriggerUpsertActionVisitor : ITriggerActionVisitor<TriggerUpsertAction>
{
    private readonly IInsertExpressionVisitor _insertExpressionVisitor;
    private readonly IUpdateExpressionVisitor _updateExpressionVisitor;
    private readonly IDbSchemaRetriever _adapter;
    private readonly IMemberInfoVisitorFactory _memberInfoVisitorFactory;
    private readonly ISqlGenerator _sqlGenerator;

    public SqlServerTriggerUpsertActionVisitor(
        IInsertExpressionVisitor insertExpressionVisitor,
        IUpdateExpressionVisitor updateExpressionVisitor,
        IDbSchemaRetriever adapter,
        IMemberInfoVisitorFactory memberInfoVisitorFactory,
        ISqlGenerator sqlGenerator)
    {
        _insertExpressionVisitor = insertExpressionVisitor;
        _updateExpressionVisitor = updateExpressionVisitor;
        _adapter = adapter;
        _memberInfoVisitorFactory = memberInfoVisitorFactory;
        _sqlGenerator = sqlGenerator;
    }

    public SqlBuilder Visit(TriggerUpsertAction triggerAction, VisitedMembers visitedMembers)
    {
        var updateEntityType = triggerAction.InsertExpression.Body.Type;
        var updateEntityTable = _adapter.GetTableName(updateEntityType);
        
        var matchExpressionParts = _memberInfoVisitorFactory.Visit(
            triggerAction.MatchExpression, triggerAction.MatchExpressionPrefixes, visitedMembers);

        var insertStatementSql = _insertExpressionVisitor.Visit(
            triggerAction.InsertExpression,
            triggerAction.InsertExpressionPrefixes,
            visitedMembers);

        var sqlBuilder = SqlBuilder.FromString("BEGIN TRANSACTION;");

        if (triggerAction.OnMatchExpression is null)
        {
            sqlBuilder.AppendNewLine($"IF NOT EXISTS(")
                .WithIdent(selectBuilder =>
                {
                    selectBuilder
                        .Append("SELECT 1")
                        .AppendNewLine($"FROM {updateEntityTable} WITH (UPDLOCK, SERIALIZABLE)")
                        .AppendNewLine("WHERE ")
                        .AppendJoin(" AND ", matchExpressionParts
                            .Select(memberPair => $"{_sqlGenerator.GetColumnSql(updateEntityType, memberPair.Key, ArgumentType.Default)} = {memberPair.Value}"))
                        .Append(")");
                });
        }
        else
        {
            var updateStatementSql = _updateExpressionVisitor.Visit(
                triggerAction.OnMatchExpression, 
                triggerAction.OnMatchExpressionPrefixes,
                visitedMembers);
            
            sqlBuilder.AppendNewLine($"UPDATE {updateEntityTable} WITH (UPDLOCK, SERIALIZABLE)")
                .AppendNewLine("SET ")
                .Append(updateStatementSql)
                .AppendNewLine("WHERE ")
                .AppendJoin(" AND ", matchExpressionParts
                    .Select(memberPair => $"{_sqlGenerator.GetColumnSql(updateEntityType, memberPair.Key, ArgumentType.Default)} = {memberPair.Value}"))
                .AppendNewLine("IF @@ROWCOUNT = 0");
        }
        
        sqlBuilder.WithIdent(actionBuilder =>
        {
            actionBuilder
                .Append($"BEGIN")
                .WithIdent(insertBuilder =>
                {
                    insertBuilder.Append($"INSERT INTO {updateEntityTable}")
                        .Append(insertStatementSql)
                        .Append(";");
                })
                .AppendNewLine("END");
        });

        sqlBuilder.AppendNewLine("COMMIT TRANSACTION;");

        return sqlBuilder;
    }
}