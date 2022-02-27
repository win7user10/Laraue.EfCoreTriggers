using System;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Services;

/// <summary>
/// SQL generator for the simplest types.
/// </summary>
public interface ISqlGenerator
{
    /// <summary>
    /// Prefix for inserted or updated (new value) entity in triggers. E.g to get balance from inserted entity, 
    /// in postgres should be used syntax NEW.balance.
    /// </summary>
    string NewEntityPrefix { get; }

    /// <summary>
    /// Prefix for deleted or updated (old value) entity in triggers. E.g to get balance from deleted entity, 
    /// in postgres should be used syntax OLD.balance.
    /// </summary>
    string OldEntityPrefix { get; }
    
    /// <summary>
    /// Get operand of expression, e.g. "+", "-", "*" etc.
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    string GetOperand(Expression expression);
    
    /// <summary>
    /// Get column SQL. E.g. NEW.column_name, OLD.column_name depending on argument
    /// and its type. 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="memberInfo"></param>
    /// <param name="argumentType"></param>
    /// <returns></returns>
    string GetColumnSql(Type type, MemberInfo memberInfo, ArgumentType argumentType);
    
    /// <summary>
    /// Get name of CLR <see cref="Type"/> in the current SQL provider.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    string GetSqlType(Type type);
    
    /// <summary>
    /// Get SQL for string, e.g. 'string'.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    string GetSql(string source);
    
    /// <summary>
    /// Get SQL for enum, e.g. 5.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    string GetSql(Enum source);
    
    /// <summary>
    /// Get SQL for bool, e.g. 0.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    string GetSql(bool source);
    
    /// <summary>
    /// Get null value SQL, e.g. NULL.
    /// </summary>
    /// <returns></returns>
    string GetNullValueSql();
    
    /// <summary>
    /// Get SQL delimiter, e.g. `.
    /// </summary>
    /// <returns></returns>
    char GetDelimiter();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    string GetVariableSql(Type type, MemberInfo member, ArgumentType argumentType);
}