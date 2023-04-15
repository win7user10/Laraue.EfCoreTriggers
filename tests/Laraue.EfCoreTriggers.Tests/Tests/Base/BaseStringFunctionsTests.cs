using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Xunit;

namespace Laraue.EfCoreTriggers.Tests.Tests.Base
{
    /// <summary>
    /// Tests of translating <see cref="string"/> functions to SQL code.
    /// </summary>
    public abstract class BaseStringFunctionsTests
    {
        /// <summary>
        /// StringField = NEW.StringField + "abc"
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> ConcatStringExpression =
            sourceEntity => new DestinationEntity
            {
                StringField = sourceEntity.New.StringField + "abc"
            };

        [Fact]
        protected abstract void StringConcatSql();

        /// <summary>
        /// StringField = NEW.StringField.ToLower()
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> StringToLowerExpression =
            sourceEntity => new DestinationEntity
            {
                StringField = sourceEntity.New.StringField.ToLower()
            };

        [Fact]
        protected abstract void StringLowerSql();

        /// <summary>
        /// StringField = NEW.StringField.ToUpper()
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> StringToUpperExpression =
            sourceEntity => new DestinationEntity
            {
                StringField = sourceEntity.New.StringField.ToUpper()
            };

        [Fact]
        protected abstract void StringUpperSql();

        /// <summary>
        /// StringField = NEW.StringField.Trim()
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> TrimStringValueExpression =
            sourceEntity => new DestinationEntity
            {
                StringField = sourceEntity.New.StringField.Trim()
            };

        [Fact]
        protected abstract void StringTrimSql();

        /// <summary>
        /// BooleanValue = NEW.StringField.Contains("abc")
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> ContainsStringValueExpression =
            sourceEntity => new DestinationEntity
            {
                BooleanValue = sourceEntity.New.StringField.Contains("abc")
            };

        [Fact]
        protected abstract void StringContainsSql();

        /// <summary>
        /// BooleanValue = NEW.StringField.EndsWith("abc")
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> EndsWithStringValueExpression =
            sourceEntity => new DestinationEntity
            {
                BooleanValue = sourceEntity.New.StringField.EndsWith("abc")
            };

        [Fact]
        protected abstract void StringEndsWithSql();

        /// <summary>
        /// BooleanValue = string.IsNullOrEmpty(NEW.StringField)
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> IsNullOrEmptyStringValueExpression =
            sourceEntity => new DestinationEntity
            {
                BooleanValue = string.IsNullOrEmpty(sourceEntity.New.StringField)
            };

        [Fact]
        protected abstract void StringIsNullOrEmptySql();
        
        /// <summary>
        /// BooleanValue = string.IsNullOrEmpty(NEW.StringField)
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> CoalesceStringExpression =
            sourceEntity => new DestinationEntity
            {
                StringField = sourceEntity.New.StringField ?? "John"
            };

        [Fact]
        protected abstract void CoalesceStringSql();
    }
}