using System;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.OnDelete;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.OnInsert;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.OnUpdate;
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
            var entityType = entityTypeBuilder.Metadata.Model.FindEntityType(typeof(T).FullName);

            var sqlProvider = TriggerExtensions.GetVisitor(entityTypeBuilder.Metadata.Model);

            var generatedSql = sqlProvider.GenerateCreateTriggerSql(configuredTrigger);
            
            entityType.AddAnnotation(configuredTrigger.Name, generatedSql);
            
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
            Action<OnUpdateTrigger<T>> configuration)
            where T : class
        {
            return entityTypeBuilder.AddOnUpdateTrigger(configuration, TriggerTime.Before);
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
            Action<OnUpdateTrigger<T>> configuration) 
            where T : class
        {
            return entityTypeBuilder.AddOnUpdateTrigger(configuration, TriggerTime.After);
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
            Action<OnUpdateTrigger<T>> configuration) 
            where T : class
        {
            return entityTypeBuilder.AddOnUpdateTrigger(configuration, TriggerTime.InsteadOf);
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
            Action<OnDeleteTrigger<T>> configuration) 
            where T : class
        {
            return entityTypeBuilder.AddOnDeleteTrigger(configuration, TriggerTime.Before);
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
            Action<OnDeleteTrigger<T>> configuration) 
            where T : class
        {
            return entityTypeBuilder.AddOnDeleteTrigger(configuration, TriggerTime.After);
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
            Action<OnDeleteTrigger<T>> configuration) 
            where T : class
        {
            return entityTypeBuilder.AddOnDeleteTrigger(configuration, TriggerTime.InsteadOf);
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
            Action<OnInsertTrigger<T>> configuration) where T : class
        {
            return entityTypeBuilder.AddOnInsertTrigger(configuration, TriggerTime.Before);
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
            Action<OnInsertTrigger<T>> configuration)
            where T : class
        {
            return entityTypeBuilder.AddOnInsertTrigger(configuration, TriggerTime.After);
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
            Action<OnInsertTrigger<T>> configuration) 
            where T : class
        {
            return entityTypeBuilder.AddOnInsertTrigger(configuration, TriggerTime.InsteadOf);
        }

        private static EntityTypeBuilder<T> AddOnUpdateTrigger<T>(this EntityTypeBuilder<T> entityTypeBuilder, Action<OnUpdateTrigger<T>> configuration,
            TriggerTime triggerTime) where T : class
        {
            var trigger = new OnUpdateTrigger<T>(triggerTime);
            
            configuration.Invoke(trigger);
            
            return entityTypeBuilder.AddTriggerAnnotation(trigger);
        }

        private static EntityTypeBuilder<T> AddOnDeleteTrigger<T>(this EntityTypeBuilder<T> entityTypeBuilder, Action<OnDeleteTrigger<T>> configuration,
            TriggerTime triggerTime) where T : class
        {
            var trigger = new OnDeleteTrigger<T>(triggerTime);
            
            configuration.Invoke(trigger);
            
            return entityTypeBuilder.AddTriggerAnnotation(trigger);
        }

        private static EntityTypeBuilder<T> AddOnInsertTrigger<T>(this EntityTypeBuilder<T> entityTypeBuilder, Action<OnInsertTrigger<T>> configuration,
            TriggerTime triggerTime) where T : class
        {
            var trigger = new OnInsertTrigger<T>(triggerTime);
            
            configuration.Invoke(trigger);
            
            return entityTypeBuilder.AddTriggerAnnotation(trigger);
        }
    }
}