using System.Linq;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Impl.SetExpressionVisitors;
using Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;

namespace Laraue.EfCoreTriggers.MySql;

public class MySqlTriggerUpsertActionVisitor : ITriggerActionVisitor<TriggerUpsertAction>
{
    private readonly ISetExpressionVisitorFactory _setExpressionVisitorFactory;
    private readonly IInsertExpressionVisitor _insertExpressionVisitor;
    private readonly IUpdateExpressionVisitor _updateExpressionVisitor;
    private readonly IEfCoreMetadataRetriever _metadataRetriever;

    public MySqlTriggerUpsertActionVisitor(
        ISetExpressionVisitorFactory setExpressionVisitorFactory, 
        IInsertExpressionVisitor insertExpressionVisitor,
        IUpdateExpressionVisitor updateExpressionVisitor,
        IEfCoreMetadataRetriever metadataRetriever)
    {
        _setExpressionVisitorFactory = setExpressionVisitorFactory;
        _insertExpressionVisitor = insertExpressionVisitor;
        _updateExpressionVisitor = updateExpressionVisitor;
        _metadataRetriever = metadataRetriever;
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
            sqlBuilder.Append($"INSERT IGNORE {_metadataRetriever.GetTableName(updateEntityType)} ")
                .Append(insertStatementSql)
                .Append(";");
        }
        else
        {
            var updateStatementSql = _updateExpressionVisitor.Visit(
                triggerAction.OnMatchExpression, 
                triggerAction.OnMatchExpressionPrefixes,
                visitedMembers);
            
            sqlBuilder.Append($"INSERT INTO {_metadataRetriever.GetTableName(updateEntityType)} ")
                .Append(insertStatementSql)
                .AppendNewLine("ON DUPLICATE KEY")
                .AppendNewLine("UPDATE ")
                .Append(updateStatementSql)
                .Append(";");
        }

        return sqlBuilder;
    }
}