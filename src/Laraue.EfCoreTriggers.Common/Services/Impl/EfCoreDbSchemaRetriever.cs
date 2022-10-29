using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Laraue.EfCoreTriggers.Common.Services.Impl;

/// <inheritdoc />
public class EfCoreDbSchemaRetriever : IDbSchemaRetriever
{
    /// <summary>
    /// Cached column names for entity properties.
    /// </summary>
    private readonly Dictionary<MemberInfo, string> _columnNamesCache = new();

    /// <summary>
    /// Cached table names for entities.
    /// </summary>
    private readonly Dictionary<Type, string> _tableNamesCache = new();

    /// <summary>
    /// Cached database schema names.
    /// </summary>
    private readonly Dictionary<Type, string> _tableSchemasCache = new();

    /// <summary>
    /// Model used for generating SQL. From this model takes column names, table names and other meta information.
    /// </summary>
    private IModel Model { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="EfCoreDbSchemaRetriever"/>.
    /// </summary>
    /// <param name="model"></param>
    public EfCoreDbSchemaRetriever(IModel model)
    {
        Model = model;
    }
    
    /// <inheritdoc />
    public string GetColumnName(Type type, MemberInfo memberInfo)
    {
        if (!_columnNamesCache.ContainsKey(memberInfo))
        {
            var entityType = GetEntityType(type);
            var property = GetColumn(type, memberInfo);
            var identifier = (StoreObjectIdentifier)StoreObjectIdentifier.Create(entityType, StoreObjectType.Table);
            _columnNamesCache.Add(memberInfo, property.GetColumnName(identifier));
        }

        if (!_columnNamesCache.TryGetValue(memberInfo, out var columnName))
        {
            throw new InvalidOperationException($"Column name for member {memberInfo.Name} is not defined in model");
        }

        return columnName;
    }

    private IProperty GetColumn(Type type, MemberInfo memberInfo)
    {
        return GetEntityType(type).FindProperty(memberInfo.Name);
    }
    
    private IEntityType GetEntityType(Type type)
    {
        var entityType = Model.FindEntityType(type);
            
        if (entityType == null)
        {
            throw new InvalidOperationException($"DbSet<{type}> should be added to the DbContext");
        }

        return entityType;
    }

    /// <inheritdoc />
    public string GetTableName(Type entity)
    {
        if (!_tableNamesCache.ContainsKey(entity))
        {
            var entityType = Model.FindEntityType(entity);
            _tableNamesCache.Add(entity, entityType.GetTableName());
        }

        if (!_tableNamesCache.TryGetValue(entity, out var tableName))
        {
            throw new InvalidOperationException($"Table name for entity {entity.FullName} is not defined in model.");
        }

        return tableName;
    }

    /// <summary>
    /// Get schema name for passed <see cref="Type">ClrType</see>.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual string GetTableSchemaName(Type entity)
    {
        if (!_tableSchemasCache.ContainsKey(entity))
        {
            var entityType = Model.FindEntityType(entity);
            _tableSchemasCache.Add(entity, entityType.GetSchema());
        }

        if (!_tableSchemasCache.TryGetValue(entity, out var schemaName))
        {
            throw new InvalidOperationException($"Schema for entity {entity.FullName} is not defined in model.");
        }

        return schemaName;
    }
    
    public PropertyInfo[] GetPrimaryKeyMembers(Type type)
    {
        var entityType = Model.FindEntityType(type);
        
        return entityType
            ?.FindPrimaryKey()
            ?.Properties
            .Select(x => x.PropertyInfo)
            .ToArray();
    }

    public KeyInfo[] GetForeignKeyMembers(Type type, Type type2)
    {
        var entityType = Model.FindEntityType(type);
        
        var outerForeignKey = entityType.GetForeignKeys()
            .FirstOrDefault(x => x.PrincipalEntityType.ClrType == type2);

        var outerKey = outerForeignKey
            .PrincipalKey
            .Properties;

        var innerKey = outerForeignKey.Properties;

        var keys = outerKey.Zip(innerKey, (first, second) => new KeyInfo
        {
            PrincipalKey = first.PropertyInfo,
            ForeignKey = second.PropertyInfo,
        }).ToArray();

        return keys;
    }

    public Type GetActualClrType(Type type, MemberInfo memberInfo)
    {
        var columnType = GetColumn(type, memberInfo);

        return columnType.FindAnnotation("ProviderClrType")?.Value as Type ?? columnType.ClrType;
    }
}