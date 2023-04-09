using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Laraue.EfCoreTriggers.Common.SqlGeneration
{
    /// <summary>
    /// The adapter allows to retrieve DB metadata about entity.
    /// </summary>
    public interface IDbSchemaRetriever
    {
        /// <summary>
        /// Get the column name of the passed member.
        /// Note: this is just a column name, without table name, quotes and schema.
        /// </summary>
        /// <param name="type">Entity type.</param>
        /// <param name="memberInfo">Member to get.</param>
        /// <returns></returns>
        string GetColumnName(Type type, MemberInfo memberInfo);
    
        /// <summary>
        /// Get the table name of passed entity.
        /// Note: this is just a table name, without quotes and schema.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        string GetTableName(Type entity);
    
        /// <summary>
        /// Return schema name for the passed entity if it is exists.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        string? GetTableSchemaName(Type entity);
    
        /// <summary>
        /// Return schema name for the passed entity if it is exists.
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        string? GetTableSchemaName(IEntityType entityType);

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
        /// <param name="type">Entity type.</param>
        /// <param name="memberInfo">Entity member.</param>
        /// <returns></returns>
        Type GetActualClrType(Type type, MemberInfo memberInfo);
    }
}