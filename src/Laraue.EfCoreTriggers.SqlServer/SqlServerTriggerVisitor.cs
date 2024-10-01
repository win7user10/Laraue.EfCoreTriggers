using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors;
using Microsoft.EntityFrameworkCore.Metadata;
using ITrigger = Laraue.EfCoreTriggers.Common.TriggerBuilders.Abstractions.ITrigger;

namespace Laraue.EfCoreTriggers.SqlServer;

public class SqlServerTriggerVisitor : BaseTriggerVisitor
{
    private readonly ITriggerActionVisitorFactory _factory;
    private readonly IDbSchemaRetriever _dbSchemaRetriever;
    private readonly ISqlGenerator _sqlGenerator;

    public SqlServerTriggerVisitor(ITriggerActionVisitorFactory factory, IDbSchemaRetriever dbSchemaRetriever, ISqlGenerator sqlGenerator)
    {
        _factory = factory;
        _dbSchemaRetriever = dbSchemaRetriever;
        _sqlGenerator = sqlGenerator;
    }

    /// <inheritdoc />
    protected override IEnumerable<TriggerTime> AvailableTriggerTimes { get; } = new[]
    {
        TriggerTime.After,
        TriggerTime.InsteadOf
    };

    /// <inheritdoc />
    public override string GenerateCreateTriggerSql(ITrigger trigger)
    {
        var triggerTimeName = GetTriggerTimeName(trigger.TriggerTime);
        var tableName = _sqlGenerator.GetTableSql(trigger.TriggerEntityType);

        var visitedMembers = new VisitedMembers();

        var actionsSql = trigger
            .Actions
            .Select(action => _factory.Visit(action, visitedMembers))
            .ToArray();

        visitedMembers.Remove(ArgumentType.Default);
        visitedMembers.Remove(ArgumentType.None);
        
        var sqlBuilder = SqlBuilder.FromString($"CREATE TRIGGER {trigger.Name} ON {tableName} {triggerTimeName} {trigger.TriggerEvent} AS")
            .AppendNewLine("BEGIN");

        sqlBuilder.WithIdent(triggerBuilder =>
        {
            var isAnyVariableDefined = visitedMembers.Any(x => x.Value.Any());
            
            if (isAnyVariableDefined)
            {
                triggerBuilder.Append(DeclareVariablesSql(trigger.TriggerEntityType, visitedMembers));
            }

            var declareCursorsSql = DeclareCursorsSql(trigger.TriggerEntityType, visitedMembers, trigger.TriggerEvent);

            if (isAnyVariableDefined)
            {
                triggerBuilder.AppendNewLine(declareCursorsSql);
            }
            else
            {
                triggerBuilder.Append(declareCursorsSql);
            }
            
            triggerBuilder.AppendNewLine(GetFetchCursorsSql(trigger.TriggerEntityType, visitedMembers, trigger.TriggerEvent))
                .AppendNewLine("WHILE @@FETCH_STATUS = 0")
                .AppendNewLine("BEGIN");
                
            sqlBuilder
                .WithIdent(triggerBodyBuilder => triggerBodyBuilder.AppendViaNewLine(actionsSql))
                .AppendNewLine(GetFetchCursorsSql(trigger.TriggerEntityType, visitedMembers, trigger.TriggerEvent))
                .AppendNewLine("END")
                .AppendNewLine(GetCloseCursorsSql(trigger.TriggerEntityType, visitedMembers, trigger.TriggerEvent));
        });

        sqlBuilder.AppendNewLine("END");

        return sqlBuilder;
    }
    
    private string DeclareVariablesSql(Type triggerEntityType, VisitedMembers visitedMembers)
    {
        var sqlBuilder = new SqlBuilder();
            
        var variablesSql = visitedMembers
            .ToDictionary(
                x => x.Key, 
                x => x.Value.WhereDeclaringType(triggerEntityType))
            .Where(x => x.Value.Any())
            .SelectMany(x => DeclareVariablesSql(triggerEntityType, x.Key, x.Value))
            .ToArray();

        if (variablesSql.Any())
        {
            sqlBuilder.Append("DECLARE ")
                .AppendJoin(", ", variablesSql);
        }
            
        return sqlBuilder;
    }
    
    private IEnumerable<string> DeclareVariablesSql(Type triggerType, ArgumentType argumentType, IEnumerable<MemberInfo> visitedMembers)
    {
        return visitedMembers
            .Select(member => GetDeclareVariableNameSql(triggerType, argumentType, member))
            .ToArray();
    }

    private string GetDeclareVariableNameSql(Type triggerType, ArgumentType argumentType, MemberInfo member)
    {
        var clrType = _dbSchemaRetriever.GetActualClrType(triggerType, member);
        
        var sqlType = _sqlGenerator.GetSqlType(clrType);
        
        return $"{GetVariableNameSql(argumentType, member)} {sqlType}";
    }

    public string GetVariableNameSql(ArgumentType argumentType, MemberInfo member)
    {
        return _sqlGenerator.GetColumnValueReferenceSql(null, member, argumentType);
    }
    
    private SqlBuilder DeclareCursorsSql(Type triggerEntityType, VisitedMembers visitedMembers, TriggerEvent triggerEvent)
    {
        return GetCursorBlockSql(triggerEntityType, visitedMembers, triggerEvent, DeclareCursorBlockSql);
    }
    
    private static SqlBuilder GetCursorBlockSql(
        Type triggerEntityType, 
        VisitedMembers visitedMembers, 
        TriggerEvent triggerEvent, 
        Func<Type, ArgumentType, MemberInfo[], SqlBuilder> generateSqlFunction)
    {
        var cursorBlocksSql = visitedMembers
            .Where(x => x.Value.WhereDeclaringType(triggerEntityType).Any())
            .Select(x => generateSqlFunction(triggerEntityType, x.Key, x.Value.ToArray()))
            .ToArray();

        // Union and return general sql for all cursors
        if (cursorBlocksSql.Any())
        {
            var sql = new SqlBuilder()
                .AppendViaNewLine(cursorBlocksSql);
            
            return sql;
        }
            
        // Generate one cursor just for iterate triggered entities
        var argumentType = triggerEvent == TriggerEvent.Delete
            ? ArgumentType.Old
            : ArgumentType.New;

        return generateSqlFunction(triggerEntityType, argumentType, Array.Empty<MemberInfo>());
    }
    
    private SqlBuilder DeclareCursorBlockSql(Type triggerEntityType, ArgumentType argumentType, MemberInfo[] affectedMembers)
    {
        var cursorName = GetCursorName(triggerEntityType, argumentType);
        
        var sqlBuilder = new SqlBuilder()
            .Append(GetDeclareCursorSql(cursorName))
            .Append(" ")
            .Append(GetSelectFromCursorSql(triggerEntityType, argumentType, affectedMembers));

        sqlBuilder.AppendNewLine($"OPEN {cursorName}");

        return sqlBuilder;
    }

    private string GetCursorName(Type triggerEntityType, ArgumentType argumentType)
    {
        return $"{GetTemporaryTableName(argumentType)}{triggerEntityType.Name}Cursor";
    }
    
    private string GetTemporaryTableName(ArgumentType argumentType)
    {
        return argumentType switch
        {
            ArgumentType.New => _sqlGenerator.NewEntityPrefix,
            ArgumentType.Old => _sqlGenerator.OldEntityPrefix,
            _ => throw new InvalidOperationException($"Temporary table for {argumentType} is not exists")
        };
    }

    private static string GetDeclareCursorSql(string cursorName)
    {
        return $"DECLARE {cursorName} CURSOR LOCAL FOR";
    }

    public override string GenerateDeleteTriggerSql(string triggerName, IEntityType entityType)
    {
        var tableSchemaPrefix = _sqlGenerator.GetSchemaPrefixSql(entityType.ClrType);
        
        return $"DROP TRIGGER {tableSchemaPrefix}{triggerName};";
    }
    
    private string GetSelectFromCursorSql(Type triggerEntityType, ArgumentType argumentType, MemberInfo[] members)
    {
        members = members.WhereDeclaringType(triggerEntityType).ToArray();
        
        var columns = members.Any()
            ? string.Join(", ", members.Select(member => _dbSchemaRetriever.GetColumnName(triggerEntityType, member)))
            : "*";
        
        return $"SELECT {columns} FROM {GetTemporaryTableName(argumentType)}";
    }
    
    private SqlBuilder GetFetchCursorsSql(Type triggerEntityType, VisitedMembers visitedMembers, TriggerEvent triggerEvent)
    {
        return GetCursorBlockSql(triggerEntityType, visitedMembers, triggerEvent, GetFetchCursorSql);
    }

    private SqlBuilder GetFetchCursorSql(Type triggerEntityType, ArgumentType argumentType, MemberInfo[] members)
    {
        var sqlBuilder = SqlBuilder.FromString($"FETCH NEXT FROM {GetCursorName(triggerEntityType, argumentType)}");

        members = members.WhereDeclaringType(triggerEntityType).ToArray();
        
        if (members.Any())
        {
            sqlBuilder.Append(" INTO ")
                .AppendJoin(", ", members.Select(member => GetVariableNameSql(argumentType, member)));
        }

        return sqlBuilder;
    }

    private static string GetCloseCursorSql(string cursorName)
    {
        return $"CLOSE {cursorName}";
    }

    private static string DeallocateCursorSql(string cursorName)
    {
        return $"DEALLOCATE {cursorName}";
    }

    private SqlBuilder GetCloseCursorsSql(Type triggerEntityType, VisitedMembers visitedMembers, TriggerEvent triggerEvent)
    {
        return GetCursorBlockSql(triggerEntityType, visitedMembers, triggerEvent, (type, argumentType, _) =>
        {
            var sqlBuilder = SqlBuilder.FromString(GetCloseCursorSql(GetCursorName(type, argumentType)))
                .Append(" ")
                .Append(DeallocateCursorSql(GetCursorName(type, argumentType)));

            return sqlBuilder;
        });
    }
}

internal static class SqlServerProviderExtensions
{
    public static IEnumerable<MemberInfo> WhereDeclaringType(this IEnumerable<MemberInfo> values, Type declaringType)
        => values.Where(x => x.ReflectedType?.IsAssignableFrom(declaringType) ?? false);
}