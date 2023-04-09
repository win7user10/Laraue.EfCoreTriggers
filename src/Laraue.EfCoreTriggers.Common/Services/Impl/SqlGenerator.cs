using System;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Laraue.EfCoreTriggers.Common.Services.Impl;

public class SqlGenerator : ISqlGenerator
{
    private readonly IDbSchemaRetriever _adapter;
    private readonly SqlTypeMappings _sqlTypeMappings;
    private readonly VisitingInfo _visitingInfo;

    public virtual string NewEntityPrefix => "NEW";

    public virtual string OldEntityPrefix => "OLD";

    /// <summary>
    /// Quote in the database to define string values.
    /// </summary>
    protected virtual char Quote => '\'';
    
    public SqlGenerator(
        IDbSchemaRetriever adapter,
        SqlTypeMappings sqlTypeMappings,
        VisitingInfo visitingInfo)
    {
        _adapter = adapter;
        _sqlTypeMappings = sqlTypeMappings;
        _visitingInfo = visitingInfo;
    }
    
    public virtual string GetNodeTypeSql(ExpressionType expressionType)
    {
        return expressionType switch
        {
            ExpressionType.Add => "+",
            ExpressionType.Subtract => "-",
            ExpressionType.Multiply => "*",
            ExpressionType.Divide => "/",
            ExpressionType.Equal => "=",
            ExpressionType.NotEqual => "<>",
            ExpressionType.AndAlso => "AND",
            ExpressionType.OrElse => "OR",
            ExpressionType.GreaterThan => ">",
            ExpressionType.GreaterThanOrEqual => ">=",
            ExpressionType.LessThan => "<",
            ExpressionType.LessThanOrEqual => "<=",
            ExpressionType.IsTrue => "IS TRUE",
            ExpressionType.IsFalse => "IS FALSE",
            ExpressionType.Negate => "-",
            ExpressionType.Not => "IS FALSE",
            ExpressionType.Quote => string.Empty,
            _ => throw new NotSupportedException($"Unknown sign of {expressionType}")
        };
    }

    public virtual string GetBinarySql(ExpressionType expressionType, SqlBuilder left, SqlBuilder right)
    {
        var nodeTypeSql = GetNodeTypeSql(expressionType);
        
        return $"{left} {nodeTypeSql} {right}";
    }

    public string GetUnarySql(ExpressionType expressionType, SqlBuilder innerExpressionSql)
    {
        var nodeTypeSql = GetNodeTypeSql(expressionType);
        
        return expressionType == ExpressionType.Negate 
            ? $"{nodeTypeSql}{innerExpressionSql}" 
            : string.IsNullOrEmpty(nodeTypeSql)
                ? innerExpressionSql
                : $"{innerExpressionSql} {nodeTypeSql}";
    }

    public string GetColumnSql(Type type, MemberInfo memberInfo, ArgumentType argumentType)
    {
        var columnSql = WrapWithDelimiters(_adapter.GetColumnName(type, memberInfo));
        
        return argumentType switch
        {
            ArgumentType.New => $"{NewEntityPrefix}.{columnSql}", 
            ArgumentType.Old => $"{OldEntityPrefix}.{columnSql}", 
            ArgumentType.None => columnSql,
            _ => $"{GetTableSql(type)}.{columnSql}",
        };
    }

    public string GetTableSql(Type entity)
    {
        var schemaName = _adapter.GetTableSchemaName(entity);
        var tableSql = WrapWithDelimiters(_adapter.GetTableName(entity));

        return string.IsNullOrWhiteSpace(schemaName)
            ? tableSql
            : $"{WrapWithDelimiters(schemaName)}.{tableSql}";
    }

    public string GetFunctionNameSql(IEntityType entityType, string name)
    {
        return GetFunctionNameSql(_adapter.GetTableSchemaName(entityType), name);
    }

    public string GetFunctionNameSql(Type entity, string name)
    {
        return GetFunctionNameSql(_adapter.GetTableSchemaName(entity), name);
    }

    private string GetFunctionNameSql(string? schemaName, string triggerName)
    {
        var functionName = WrapWithDelimiters(triggerName);

        return string.IsNullOrWhiteSpace(schemaName)
            ? functionName
            : $"{WrapWithDelimiters(schemaName)}.{functionName}";
    }

    public string GetSqlType(Type type)
    {
        type = EfCoreTriggersHelper.GetNotNullableType(type);
        type = type.IsEnum ? typeof(Enum) : type;
        _sqlTypeMappings.TryGetValue(type, out var sqlType);
        return sqlType;
    }

    public string GetSql(string source)
    {
        return $"{Quote}{source}{Quote}";
    }

    public string GetSql(char source)
    {
        return $"{Quote}{source}{Quote}";
    }

    public string GetSql(Enum source)
    {
        var clrType = _adapter.GetActualClrType(
            _visitingInfo.CurrentMember.DeclaringType,
            _visitingInfo.CurrentMember);

        return clrType == typeof(string)
            ? GetSql(source.ToString())
            : source.ToString("D");
    }

    public virtual string GetSql(bool source)
    {
        return $"{source.ToString().ToLower()}";
    }

    public string GetNullValueSql()
    {
        return "NULL";
    }

    public virtual char GetDelimiter()
    {
        return '"';
    }

    public virtual string GetColumnValueReferenceSql(Type type, MemberInfo member, ArgumentType argumentType)
    {
        return GetColumnSql(type, member, argumentType);
    }

    private string WrapWithDelimiters(string value)
    {
        return $"{GetDelimiter()}{value}{GetDelimiter()}";
    }
}