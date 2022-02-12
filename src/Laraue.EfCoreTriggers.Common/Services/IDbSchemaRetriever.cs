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
    /// Get all members which are used in primary key.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    PropertyInfo[] GetPrimaryKeyMembers(Type type);
}