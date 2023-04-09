using System;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Laraue.EfCoreTriggers.Common.SqlGeneration
{
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
        /// <param name="expressionType">Binary expression.</param>
        /// <param name="left">Sql of the left side expression.</param>
        /// <param name="right">Sql of the right side expression.</param>
        /// <returns></returns>
        string GetBinarySql(ExpressionType expressionType, SqlBuilder left, SqlBuilder right);
    
        /// <summary>
        /// Get operand of expression, e.g. "+", "-", "*" etc.
        /// </summary>
        /// <param name="expressionType">Unary expression.</param>
        /// <param name="innerExpressionSql">Unary expression sql.</param>
        /// <returns></returns>
        string GetUnarySql(ExpressionType expressionType, SqlBuilder innerExpressionSql);
    
        /// <summary>
        /// Get column SQL. E.g. NEW."column_name", OLD."column_name" depending on argument
        /// and its type. 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberInfo"></param>
        /// <param name="argumentType"></param>
        /// <returns></returns>
        string GetColumnSql(Type type, MemberInfo memberInfo, ArgumentType argumentType);

        /// <summary>
        /// Get table SQL, e.g. "dbo"."Users".
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        string GetTableSql(Type entity);
    
        /// <summary>
        /// Get the function name with the entities schema.
        /// User, "AFTER_DELETE" -> "schema"."AFTER_DELETE_USER"
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetFunctionNameSql(IEntityType entityType, string name);
    
        /// <summary>
        /// <see cref="GetFunctionNameSql(Microsoft.EntityFrameworkCore.Metadata.IEntityType,string)"/>
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetFunctionNameSql(Type entityType, string name);
    
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
        /// Get SQL for char, e.g. 'a'.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        string GetSql(char source);
    
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
        /// Get SQL to access a column value. In SQL Server that returns a
        /// reference to the defined variable, e.g. @NewAge, in other dialects - reference to a
        /// column name, e.g. NEW."Age".
        /// </summary>
        /// <returns></returns>
        string GetColumnValueReferenceSql(Type? type, MemberInfo member, ArgumentType argumentType);
    }
}