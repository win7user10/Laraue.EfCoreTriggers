using System.Linq;
using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.TriggerBuilders;
using Laraue.Linq2Triggers.Core.TriggerBuilders.Actions;
using Laraue.Linq2Triggers.Core.Visitors.SetExpressionVisitors;
using Laraue.Linq2Triggers.Core.Visitors.TriggerVisitors;

namespace Laraue.Linq2Triggers.Providers.Oracle;

/// <inheritdoc />
public sealed class OracleTriggerUpsertActionVisitor : ITriggerActionVisitor<TriggerUpsertAction>
{
    private readonly ISqlGenerator _sqlGenerator;
    private readonly IMemberInfoVisitorFactory _factory;

    public OracleTriggerUpsertActionVisitor(
        ISqlGenerator sqlGenerator,
        IMemberInfoVisitorFactory factory)
    {
        _sqlGenerator = sqlGenerator;
        _factory = factory;
    }
    
    /// <inheritdoc />
    public SqlBuilder Visit(TriggerUpsertAction triggerAction, VisitedMembers visitedMembers)
    {
        var updateEntityType = triggerAction.InsertExpression.Body.Type;

        var sqlBuilder = new SqlBuilder();
        
        var insertParts = _factory.Visit(
            triggerAction.InsertExpression,
            visitedMembers);
        
        var matchParts = _factory.Visit(
            triggerAction.MatchExpression,
            visitedMembers);

        var targetTableSql = _sqlGenerator.GetTableSql(updateEntityType);
        
        sqlBuilder
            .Append("MERGE INTO ")
            .Append(targetTableSql)
            .AppendNewLine("USING (")
            .WithIdent(builder =>
            {
                builder
                    .Append("SELECT ")
                    .WithIdent(selectBuilder =>
                    {
                        selectBuilder
                            .AppendViaNewLine(", ", insertParts
                                .Select(x => x.Value));
                    });
            })
            .AppendNewLine(")");

        sqlBuilder
            .AppendNewLine("ON (")
            .AppendViaNewLine(", ", matchParts
                .Select(x => x.Value))
            .Append(")");
            
        sqlBuilder.AppendNewLine("WHEN NOT MATCHED THEN");
        sqlBuilder.AppendNewLine("INSERT (")
            .WithIdent(insertBuilder =>
            {
                insertBuilder
                    .AppendJoin(", ", insertParts
                        .Select(x =>
                            _sqlGenerator.GetColumnSql(updateEntityType, x.Key.Name, ArgumentType.None)));
            })
            .AppendNewLine(")");
            
        sqlBuilder
            .AppendNewLine("VALUES (")
            .WithIdent(valuesBuilder =>
            {
                valuesBuilder.AppendViaNewLine(", ", insertParts
                    .Select(x => x.Value));
            })
            .AppendNewLine(")");

        if (triggerAction.UpdateExpression is not null)
        {
            sqlBuilder.AppendNewLine("WHEN MATCHED THEN");
            sqlBuilder.AppendNewLine("UPDATE SET")
                .WithIdent(updateBuilder =>
                {
                    var updateParts = _factory.Visit(
                        triggerAction.UpdateExpression,
                        visitedMembers);
                
                    updateBuilder
                        .AppendJoin(", ", updateParts
                            .Select(x =>
                            {
                                var columnSql = _sqlGenerator.GetColumnSql(
                                    updateEntityType,
                                    x.Key.Name,
                                    ArgumentType.None);
                            
                                return $"{columnSql} = {x.Value}";
                            }));
                });
        }

        sqlBuilder.Append(";");
        
        return sqlBuilder;
    }
}