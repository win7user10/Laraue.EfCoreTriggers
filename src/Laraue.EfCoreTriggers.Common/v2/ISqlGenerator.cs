using System;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.v2;

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
    
    string GetOperand(Expression expression);
    string GetColumnSql(MemberInfo memberInfo, ArgumentType argumentType);
    string GetSqlType(Type type);
    string GetSql(string source);
    string GetSql(Enum source);
    string GetSql(bool source);
    string GetNullValueSql();
    char GetDelimiter();
    Type GetNotNullableType(Type type);
}