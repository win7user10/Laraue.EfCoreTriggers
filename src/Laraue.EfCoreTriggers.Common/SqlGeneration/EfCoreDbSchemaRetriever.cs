using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Laraue.EfCoreTriggers.Common.SqlGeneration
{
    /// <inheritdoc />
    public class EfCoreDbSchemaRetriever : IDbSchemaRetriever
    {
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
            var entityType = GetEntityType(type);
            var column = GetColumn(type, memberInfo);
        
            var identifier = (StoreObjectIdentifier)StoreObjectIdentifier.Create(entityType, StoreObjectType.Table)!;
            return column.GetColumnName(identifier) 
                   ?? throw new InvalidOperationException($"Column information was not found for {identifier}");
        }

        private IProperty GetColumn(Type type, MemberInfo memberInfo)
        {
            return GetEntityType(type).FindProperty(memberInfo.Name)
                   ?? throw new InvalidOperationException($"Column {memberInfo.Name} was not found in {type}");
        }
    
        private IEntityType GetEntityType(Type type)
        {
            var entityType = Model.FindEntityType(type);
            
            if (entityType == null)
            {
                throw new InvalidOperationException($"DbSet<{type.FullName}> should be added to the DbContext");
            }

            return entityType;
        }

        /// <inheritdoc />
        public string GetTableName(Type type)
        {
            return GetEntityType(type).GetTableName()
                   ?? throw new InvalidOperationException($"{type} is not mapped to the table");
        }

        /// <summary>
        /// Get schema name for passed <see cref="Type">ClrType</see>.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual string? GetTableSchemaName(Type entity)
        {
            return GetTableSchemaName(GetEntityType(entity));
        }

        /// <inheritdoc />
        public string? GetTableSchemaName(IEntityType? entityType)
        {
            return entityType?.GetSchema();
        }

        /// <inheritdoc />
        public PropertyInfo[] GetPrimaryKeyMembers(Type type)
        {
            var entityType = Model.FindEntityType(type);
        
            return entityType
                       ?.FindPrimaryKey()
                       ?.Properties
                       .Select(x => x.PropertyInfo!)
                       .ToArray()
                   ?? Array.Empty<PropertyInfo>();
        }

        /// <inheritdoc />
        public KeyInfo[] GetForeignKeyMembers(Type type, Type type2)
        {
            var entityType = Model.FindEntityType(type);
        
            var outerForeignKey = entityType.GetForeignKeys()
                .First(x => x.PrincipalEntityType.ClrType == type2);

            var outerKey = outerForeignKey
                .PrincipalKey
                .Properties;

            var innerKey = outerForeignKey.Properties;

            var keys = outerKey
                .Zip(innerKey, (first, second)
                    => new KeyInfo(
                        first.PropertyInfo,
                        second.PropertyInfo))
                .ToArray();

            return keys;
        }

        /// <inheritdoc />
        public Type GetActualClrType(Type type, MemberInfo memberInfo)
        {
            var columnType = GetColumn(type, memberInfo);

            return columnType.FindAnnotation("ProviderClrType")?.Value as Type ?? columnType.ClrType;
        }
    }
}