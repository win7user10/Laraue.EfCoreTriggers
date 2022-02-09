using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.v2.Impl;

public class SqlGenerator : ISqlGenerator
{
    private readonly IEfCoreMetadataRetriever _metadataRetriever;
    private readonly SqlTypeMappings _sqlTypeMappings;

    public virtual string NewEntityPrefix => "NEW";

    public virtual string OldEntityPrefix => "OLD";

    /// <summary>
    /// Quote in the database.
    /// </summary>
    protected virtual char Quote => '\'';
    
    public SqlGenerator(IEfCoreMetadataRetriever metadataRetriever, SqlTypeMappings sqlTypeMappings)
    {
        _metadataRetriever = metadataRetriever;
        _sqlTypeMappings = sqlTypeMappings;
    }
    
    public virtual string GetOperand(Expression expression)
    {
        return expression.NodeType switch
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
            ExpressionType.IsTrue => "is true",
            ExpressionType.IsFalse => "is false",
            ExpressionType.Negate => "-",
            ExpressionType.Not => "is false",
            _ => throw new NotSupportedException($"Unknown sign of {expression.NodeType}")
        };
    }

    public string GetColumnSql(MemberInfo memberInfo, ArgumentType argumentType)
    {
        var columnName = _metadataRetriever.GetColumnName(memberInfo);
        
        return argumentType switch
        {
            ArgumentType.New => $"{NewEntityPrefix}.{columnName}", 
            ArgumentType.Old => $"{OldEntityPrefix}.{columnName}", 
            ArgumentType.None => columnName,
            _ => $"{_metadataRetriever.GetTableName(memberInfo.DeclaringType)}.{columnName}",
        };
    }

    public string GetSqlType(Type type)
    {
        type = GetNotNullableType(type);
        type = type.IsEnum ? typeof(Enum) : type;
        _sqlTypeMappings.TryGetValue(type, out var sqlType);
        return sqlType;
    }
    
    public Type GetNotNullableType(Type type)
    {
        var nullableUnderlyingType = Nullable.GetUnderlyingType(type);
        
        return nullableUnderlyingType ?? type;
    }

    public string GetSql(string source)
    {
        return $"{Quote}{source}{Quote}";
    }

    public string GetSql(Enum source)
    {
        return source.ToString("D");
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
}