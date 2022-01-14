﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Laraue.EfCoreTriggers.Common.v2.Impl;

public class EfCoreMetadataRetriever : IEfCoreMetadataRetriever
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
    protected IReadOnlyModel Model { get; }

    public EfCoreMetadataRetriever(IReadOnlyModel model)
    {
        Model = model;
    }
    
    public string GetColumnName(MemberInfo memberInfo)
    {
        if (!_columnNamesCache.ContainsKey(memberInfo))
        {
            var declaringType = memberInfo.DeclaringType;
            var entityType = Model.FindEntityType(declaringType);
            if (entityType == null)
            {
                throw new InvalidOperationException($"DbSet<{declaringType.Name}> should be added to the DbContext");
            }
            
            var property = entityType.FindProperty(memberInfo.Name);
            var identifier = (StoreObjectIdentifier)StoreObjectIdentifier.Create(entityType, StoreObjectType.Table);
            _columnNamesCache.Add(memberInfo, property.GetColumnName(identifier));
        }

        if (!_columnNamesCache.TryGetValue(memberInfo, out var columnName))
        {
            throw new InvalidOperationException($"Column name for member {memberInfo.Name} is not defined in model");
        }

        return columnName;
    }

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

        var schemaName = GetTableSchemaName(entity);

        return string.IsNullOrWhiteSpace(schemaName)
            ? tableName
            : $"{schemaName}.{tableName}";
    }
    
    /// <summary>
    /// Get schema name for passed <see cref="Type">ClrType</see>.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    protected virtual string GetTableSchemaName(Type entity)
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
}