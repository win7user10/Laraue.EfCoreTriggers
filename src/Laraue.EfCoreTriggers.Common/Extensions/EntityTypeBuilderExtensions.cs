using System;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Laraue.EfCoreTriggers.Common.Extensions
{
    /// <summary>
    /// Extensions to configure triggers.
    /// </summary>
    public static class EntityTypeBuilderExtensions
    {
        /// <summary>
        /// Generate SQL for configured via <see cref="EntityTypeBuilder{T}"/> <see cref="ITrigger"/>
        /// and append it as annotation to the entity.
        /// </summary>
        /// <param name="entityTypeBuilder"></param>
        /// <param name="configuredTrigger"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static EntityTypeBuilder<T> AddTriggerAnnotation<T>(
            this EntityTypeBuilder<T> entityTypeBuilder,
            ITrigger configuredTrigger) where T : class
        {
            var entityTypeName = typeof(T).FullName!;
            var entityType = entityTypeBuilder.Metadata.Model.FindEntityType(entityTypeName)
                ?? throw new InvalidOperationException($"Entity {entityTypeName} metadata was not found");

#if NET6_0_OR_GREATER
            entityTypeBuilder.ToTable(tb => tb.HasTrigger(configuredTrigger.Name));
#endif
            entityType.AddAnnotation(configuredTrigger.Name, configuredTrigger);
            
            return entityTypeBuilder;
        }

        /// <summary>
        /// Execute SQL before entity will be updated.
        /// </summary>
        /// <param name="entityTypeBuilder">Entity builder.</param>
        /// <param name="configuration">Action to execute.</param>
        /// <typeparam name="T">Entity should be updated.</typeparam>
        /// <returns></returns>
        public static EntityTypeBuilder<T> BeforeUpdate<T>(
            this EntityTypeBuilder<T> entityTypeBuilder,
            Action<NewTrigger<T, OldAndNewTableRefs<T>>> configuration)
            where T : class
        {
            return entityTypeBuilder.AddTrigger(
                TriggerEvent.Update,
                TriggerTime.Before,
                configuration);
        }

        /// <summary>
        /// Execute SQL after entity updated.
        /// </summary>
        /// <param name="entityTypeBuilder">Entity builder.</param>
        /// <param name="configuration">Action to execute.</param>
        /// <typeparam name="T">Entity that was updated.</typeparam>
        /// <returns></returns>
        public static EntityTypeBuilder<T> AfterUpdate<T>(
            this EntityTypeBuilder<T> entityTypeBuilder,
            Action<NewTrigger<T, OldAndNewTableRefs<T>>> configuration) 
            where T : class
        {
            return entityTypeBuilder.AddTrigger(
                TriggerEvent.Update,
                TriggerTime.After,
                configuration);
        }
        
        /// <summary>
        /// Execute SQL instead of entity update.
        /// </summary>
        /// <param name="entityTypeBuilder">Entity builder.</param>
        /// <param name="configuration">Action to execute.</param>
        /// <typeparam name="T">Entity should be updated.</typeparam>
        /// <returns></returns>
        public static EntityTypeBuilder<T> InsteadOfUpdate<T>(
            this EntityTypeBuilder<T> entityTypeBuilder,
            Action<NewTrigger<T, OldAndNewTableRefs<T>>> configuration) 
            where T : class
        {
            return entityTypeBuilder.AddTrigger(
                TriggerEvent.Update,
                TriggerTime.InsteadOf,
                configuration);
        }
        
        /// <summary>
        /// Execute SQL before entity delete.
        /// </summary>
        /// <param name="entityTypeBuilder">Entity builder.</param>
        /// <param name="configuration">Action to execute.</param>
        /// <typeparam name="T">Entity should be deleted.</typeparam>
        /// <returns></returns>
        public static EntityTypeBuilder<T> BeforeDelete<T>(
            this EntityTypeBuilder<T> entityTypeBuilder,
            Action<NewTrigger<T, OldTableRef<T>>> configuration) 
            where T : class
        {
            return entityTypeBuilder.AddTrigger(
                TriggerEvent.Delete,
                TriggerTime.Before,
                configuration);
        }
        
        /// <summary>
        /// Execute SQL after entity delete.
        /// </summary>
        /// <param name="entityTypeBuilder">Entity builder.</param>
        /// <param name="configuration">Action to execute.</param>
        /// <typeparam name="T">Entity that was deleted.</typeparam>
        /// <returns></returns>
        public static EntityTypeBuilder<T> AfterDelete<T>(
            this EntityTypeBuilder<T> entityTypeBuilder,
            Action<NewTrigger<T, OldTableRef<T>>> configuration) 
            where T : class
        {
            return entityTypeBuilder.AddTrigger(
                TriggerEvent.Delete,
                TriggerTime.After,
                configuration);
        }

        /// <summary>
        /// Execute SQL instead of entity delete.
        /// </summary>
        /// <param name="entityTypeBuilder">Entity builder.</param>
        /// <param name="configuration">Action to execute.</param>
        /// <typeparam name="T">Entity should be deleted.</typeparam>
        /// <returns></returns>
        public static EntityTypeBuilder<T> InsteadOfDelete<T>(
            this EntityTypeBuilder<T> entityTypeBuilder,
            Action<NewTrigger<T, OldTableRef<T>>> configuration) 
            where T : class
        {
            return entityTypeBuilder.AddTrigger(
                TriggerEvent.Delete,
                TriggerTime.InsteadOf,
                configuration);
        }

        /// <summary>
        /// Execute SQL before entity insert.
        /// </summary>
        /// <param name="entityTypeBuilder">Entity builder.</param>
        /// <param name="configuration">Action to execute.</param>
        /// <typeparam name="T">Entity should be inserted.</typeparam>
        /// <returns></returns>
        public static EntityTypeBuilder<T> BeforeInsert<T>(
            this EntityTypeBuilder<T> entityTypeBuilder,
            Action<NewTrigger<T, NewTableRef<T>>> configuration)
            where T : class
        {
            return entityTypeBuilder.AddTrigger(
                TriggerEvent.Insert,
                TriggerTime.Before,
                configuration);
        }

        /// <summary>
        /// Execute SQL after entity insert.
        /// </summary>
        /// <param name="entityTypeBuilder">Entity builder.</param>
        /// <param name="configuration">Action to execute.</param>
        /// <typeparam name="T">Entity that was inserted.</typeparam>
        /// <returns></returns>
        public static EntityTypeBuilder<T> AfterInsert<T>(
            this EntityTypeBuilder<T> entityTypeBuilder,
            Action<NewTrigger<T, NewTableRef<T>>> configuration)
            where T : class
        {
            return entityTypeBuilder.AddTrigger(
                TriggerEvent.Insert,
                TriggerTime.After,
                configuration);
        }

        /// <summary>
        /// Execute SQL instead of entity insert.
        /// </summary>
        /// <param name="entityTypeBuilder">Entity builder.</param>
        /// <param name="configuration">Action to execute.</param>
        /// <typeparam name="T">Entity should be inserted.</typeparam>
        /// <returns></returns>
        public static EntityTypeBuilder<T> InsteadOfInsert<T>(
            this EntityTypeBuilder<T> entityTypeBuilder,
            Action<NewTrigger<T, NewTableRef<T>>> configuration) 
            where T : class
        {
            return entityTypeBuilder.AddTrigger(
                TriggerEvent.Insert,
                TriggerTime.InsteadOf,
                configuration);
        }

        private static EntityTypeBuilder<T> AddTrigger<T, TRefs>(
            this EntityTypeBuilder<T> entityTypeBuilder,
            TriggerEvent triggerEvent,
            TriggerTime triggerTime,
            Action<NewTrigger<T, TRefs>> configureTrigger)
            where T : class
            where TRefs : ITableRef<T>
        {
            var trigger = new NewTrigger<T, TRefs>(triggerEvent, triggerTime);
            
            configureTrigger.Invoke(trigger);
            
            return entityTypeBuilder.AddTriggerAnnotation(trigger);
        }
    }
}