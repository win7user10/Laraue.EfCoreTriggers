using System;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Services.Impl;

public class SqlGenerator : ISqlGenerator
{
    private readonly IDbSchemaRetriever _adapter;
    private readonly SqlTypeMappings _sqlTypeMappings;

    public virtual string NewEntityPrefix => "NEW";

    public virtual string OldEntityPrefix => "OLD";

    /// <summary>
    /// Quote in the database.
    /// </summary>
    protected virtual char Quote => '\'';
    
    public SqlGenerator(IDbSchemaRetriever adapter, SqlTypeMappings sqlTypeMappings)
    {
        _adapter = adapter;
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

    public string GetColumnSql(Type type, MemberInfo memberInfo, ArgumentType argumentType)
    {
        var columnName = _adapter.GetColumnName(type, memberInfo);
        
        return argumentType switch
        {
            ArgumentType.New => $"{NewEntityPrefix}.{columnName}", 
            ArgumentType.Old => $"{OldEntityPrefix}.{columnName}", 
            ArgumentType.None => columnName,
            _ => $"{_adapter.GetTableName(type)}.{columnName}",
        };
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