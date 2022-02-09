using System.Linq;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Impl.SetExpressionVisitors;
using Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;

namespace Laraue.EfCoreTriggers.SqlServer;

public class SqlServerTriggerUpsertActionVisitor : ITriggerActionVisitor<TriggerUpsertAction>
{
    private readonly IInsertExpressionVisitor _insertExpressionVisitor;
    private readonly IUpdateExpressionVisitor _updateExpressionVisitor;
    private readonly IEfCoreMetadataRetriever _metadataRetriever;
    private readonly ISetExpressionVisitorFactory _setExpressionVisitorFactory;
    private readonly ISqlGenerator _sqlGenerator;

    public SqlServerTriggerUpsertActionVisitor(IInsertExpressionVisitor insertExpressionVisitor, IUpdateExpressionVisitor updateExpressionVisitor, IEfCoreMetadataRetriever metadataRetriever, ISetExpressionVisitorFactory setExpressionVisitorFactory, ISqlGenerator sqlGenerator)
    {
        _insertExpressionVisitor = insertExpressionVisitor;
        _updateExpressionVisitor = updateExpressionVisitor;
        _metadataRetriever = metadataRetriever;
        _setExpressionVisitorFactory = setExpressionVisitorFactory;
        _sqlGenerator = sqlGenerator;
    }

    public SqlBuilder Visit(TriggerUpsertAction triggerAction, VisitedMembers visitedMembers)
    {
        var updateEntityType = triggerAction.InsertExpression.Body.Type;
        var updateEntityTable = _metadataRetriever.GetTableName(updateEntityType);
        
        var matchExpressionParts = _setExpressionVisitorFactory.Visit(
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
                            .Select(memberPair => $"{_sqlGenerator.GetColumnSql(memberPair.Key, ArgumentType.Default)} = {memberPair.Value}"))
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
                    .Select(memberPair => $"{_sqlGenerator.GetColumnSql(memberPair.Key, ArgumentType.Default)} = {memberPair.Value}"))
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