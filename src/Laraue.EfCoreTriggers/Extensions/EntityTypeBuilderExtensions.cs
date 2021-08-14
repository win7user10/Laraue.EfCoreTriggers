using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;
using Laraue.EfCoreTriggers.Common.Builders.Triggers.OnDelete;
using Laraue.EfCoreTriggers.Common.Builders.Triggers.OnInsert;
using Laraue.EfCoreTriggers.Common.Builders.Triggers.OnUpdate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Laraue.EfCoreTriggers.Common;

namespace Laraue.EfCoreTriggers.Extensions
{
    public static class EntityTypeBuilderExtensions
    {
        private static EntityTypeBuilder<T> AddTriggerAnnotation<T>(
            this EntityTypeBuilder<T> entityTypeBuilder,
            Trigger<T> configuredTrigger) where T : class
        {
            var entityType = entityTypeBuilder.Metadata.Model.FindEntityType(typeof(T).FullName);
            entityType.AddAnnotation(configuredTrigger.Name, configuredTrigger.BuildSql(TriggerExtensions.GetSqlProvider(entityTypeBuilder.Metadata.Model)).Sql);
            return entityTypeBuilder;
        }

        public static EntityTypeBuilder<T> BeforeUpdate<T>(this EntityTypeBuilder<T> entityTypeBuilder, Action<OnUpdateTrigger<T>> configuration) where T : class
            => entityTypeBuilder.AddOnUpdateTrigger(configuration, TriggerTime.Before);

        public static EntityTypeBuilder<T> AfterUpdate<T>(this EntityTypeBuilder<T> entityTypeBuilder, Action<OnUpdateTrigger<T>> configuration) where T : class
            => entityTypeBuilder.AddOnUpdateTrigger(configuration, TriggerTime.After);

        public static EntityTypeBuilder<T> InsteadOfUpdate<T>(this EntityTypeBuilder<T> entityTypeBuilder, Action<OnUpdateTrigger<T>> configuration) where T : class
            => entityTypeBuilder.AddOnUpdateTrigger(configuration, TriggerTime.InsteadOf);

        public static EntityTypeBuilder<T> BeforeDelete<T>(this EntityTypeBuilder<T> entityTypeBuilder, Action<OnDeleteTrigger<T>> configuration) where T : class
            => entityTypeBuilder.AddOnDeleteTrigger(configuration, TriggerTime.Before);

        public static EntityTypeBuilder<T> AfterDelete<T>(this EntityTypeBuilder<T> entityTypeBuilder, Action<OnDeleteTrigger<T>> configuration) where T : class
            => entityTypeBuilder.AddOnDeleteTrigger(configuration, TriggerTime.After);

        public static EntityTypeBuilder<T> InsteadOfDelete<T>(this EntityTypeBuilder<T> entityTypeBuilder, Action<OnDeleteTrigger<T>> configuration) where T : class
            => entityTypeBuilder.AddOnDeleteTrigger(configuration, TriggerTime.InsteadOf);

        public static EntityTypeBuilder<T> BeforeInsert<T>(this EntityTypeBuilder<T> entityTypeBuilder, Action<OnInsertTrigger<T>> configuration) where T : class
            => entityTypeBuilder.AddOnInsertTrigger(configuration, TriggerTime.Before);

        public static EntityTypeBuilder<T> AfterInsert<T>(this EntityTypeBuilder<T> entityTypeBuilder, Action<OnInsertTrigger<T>> configuration) where T : class
            => entityTypeBuilder.AddOnInsertTrigger(configuration, TriggerTime.After);

        public static EntityTypeBuilder<T> InsteadOfInsert<T>(this EntityTypeBuilder<T> entityTypeBuilder, Action<OnInsertTrigger<T>> configuration) where T : class
            => entityTypeBuilder.AddOnInsertTrigger(configuration, TriggerTime.InsteadOf);

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