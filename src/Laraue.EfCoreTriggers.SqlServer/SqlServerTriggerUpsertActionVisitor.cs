using System.Linq;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Actions;
using Laraue.EfCoreTriggers.Common.Visitors.SetExpressionVisitors;
using Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors;
using Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors.Statements;

namespace Laraue.EfCoreTriggers.SqlServer;

public class SqlServerTriggerUpsertActionVisitor : ITriggerActionVisitor<TriggerUpsertAction>
{
    private readonly IInsertExpressionVisitor _insertExpressionVisitor;
    private readonly IUpdateExpressionVisitor _updateExpressionVisitor;
    private readonly IMemberInfoVisitorFactory _memberInfoVisitorFactory;
    private readonly ISqlGenerator _sqlGenerator;

    public SqlServerTriggerUpsertActionVisitor(
        IInsertExpressionVisitor insertExpressionVisitor,
        IUpdateExpressionVisitor updateExpressionVisitor,
        IMemberInfoVisitorFactory memberInfoVisitorFactory,
        ISqlGenerator sqlGenerator)
    {
        _insertExpressionVisitor = insertExpressionVisitor;
        _updateExpressionVisitor = updateExpressionVisitor;
        _memberInfoVisitorFactory = memberInfoVisitorFactory;
        _sqlGenerator = sqlGenerator;
    }

    public SqlBuilder Visit(TriggerUpsertAction triggerAction, VisitedMembers visitedMembers)
    {
        var updateEntityType = triggerAction.InsertExpression.Body.Type;
        var updateEntityTable = _sqlGenerator.GetTableSql(updateEntityType);
        
        var matchExpressionParts = _memberInfoVisitorFactory.Visit(
            triggerAction.MatchExpression, visitedMembers);

        var insertStatementSql = _insertExpressionVisitor.Visit(
            triggerAction.InsertExpression,
            visitedMembers);

        var sqlBuilder = SqlBuilder.FromString("BEGIN TRANSACTION;");

        if (triggerAction.UpdateExpression is null)
        {
            sqlBuilder.AppendNewLine($"IF NOT EXISTS(")
                .WithIdent(selectBuilder =>
                {
                    selectBuilder
                        .Append("SELECT 1")
                        .AppendNewLine($"FROM {updateEntityTable} WITH (UPDLOCK, SERIALIZABLE)")
                        .AppendNewLine("WHERE ")
                        .AppendJoin(" AND ", matchExpressionParts
                            .Select(memberPair => memberPair.Value))
                        .Append(")");
                });
        }
        else
        {
            var updateStatementSql = _updateExpressionVisitor.Visit(
                triggerAction.UpdateExpression,
                visitedMembers);
            
            sqlBuilder.AppendNewLine($"UPDATE {updateEntityTable} WITH (UPDLOCK, SERIALIZABLE)")
                .AppendNewLine("SET ")
                .Append(updateStatementSql)
                .AppendNewLine("WHERE ")
                .AppendJoin(" AND ", matchExpressionParts.Select(x => x.Value))
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