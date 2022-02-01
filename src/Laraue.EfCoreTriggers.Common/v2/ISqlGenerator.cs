using System;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.v2;

public interface ISqlGenerator
{
    string GetOperand(Expression expression);
    string GetColumnSql(MemberInfo memberInfo, ArgumentType argumentType);
    string GetSqlType(Type type);
    string GetSql(string source);
    string GetSql(Enum source);
    string GetSql(bool source);
    string GetNullValueSql();
    char GetDelimiter();
}