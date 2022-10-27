using System;
using System.Reflection;

namespace Laraue.EfCoreTriggers.Common.Services;

/// <summary>
/// The adapter allows to retrieve DB metadata about entity.
/// </summary>
public interface IDbSchemaRetriever
{
    /// <summary>
    /// Get the column name of the passed member.
    /// </summary>
    /// <param name="type">Entity type.</param>
    /// <param name="memberInfo">Member to get.</param>
    /// <returns></returns>
    string GetColumnName(Type type, MemberInfo memberInfo);
    
    /// <summary>
    /// Get the table name of passed entity.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    string GetTableName(Type entity);

    /// <summary>
    /// Get the function name with the entities schema.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    string GetFunctionName(Type entity, string name);

    /// <summary>
    /// Get all members which are used in primary key.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    PropertyInfo[] GetPrimaryKeyMembers(Type type);

    /// <summary>
    /// Get info about cases participating in relations between two types.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="type2"></param>
    /// <returns></returns>
    KeyInfo[] GetForeignKeyMembers(Type type, Type type2);

    /// <summary>
    /// Some type can be overriden, for example Enum can be store as string in the DB.
    /// In these cases clr type will be returned from this function.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="memberInfo"></param>
    /// <param name="clrType"></param>
    /// <returns></returns>
    bool TryGetActualClrType(Type type, MemberInfo memberInfo, out Type clrType);
}