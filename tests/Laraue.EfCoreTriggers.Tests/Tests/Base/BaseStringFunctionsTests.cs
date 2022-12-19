using System;
using System.Linq.Expressions;
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
        /// StringField = OLD.StringField + "abc"
        /// </summary>
        protected Expression<Func<SourceEntity, DestinationEntity>> ConcatStringExpression = sourceEntity => new DestinationEntity
        {
            StringField = sourceEntity.StringField + "abc"
        };

        [Fact]
        protected abstract void StringConcatSql();

        /// <summary>
        /// StringField = OLD.StringField.ToLower()
        /// </summary>
        protected Expression<Func<SourceEntity, DestinationEntity>> StringToLowerExpression = sourceEntity => new DestinationEntity
        {
            StringField = sourceEntity.StringField.ToLower()
        };

        [Fact]
        protected abstract void StringLowerSql();

        /// <summary>
        /// StringField = OLD.StringField.ToUpper()
        /// </summary>
        protected Expression<Func<SourceEntity, DestinationEntity>> StringToUpperExpression = sourceEntity => new DestinationEntity
        {
            StringField = sourceEntity.StringField.ToUpper()
        };

        [Fact]
        protected abstract void StringUpperSql();

        /// <summary>
        /// StringField = OLD.StringField.Trim()
        /// </summary>
        protected Expression<Func<SourceEntity, DestinationEntity>> TrimStringValueExpression = sourceEntity => new DestinationEntity
        {
            StringField = sourceEntity.StringField.Trim()
        };

        [Fact]
        protected abstract void StringTrimSql();

        /// <summary>
        /// BooleanValue = OLD.StringField.Contains("abc")
        /// </summary>
        protected Expression<Func<SourceEntity, DestinationEntity>> ContainsStringValueExpression = sourceEntity => new DestinationEntity
        {
            BooleanValue = sourceEntity.StringField.Contains("abc")
        };

        [Fact]
        protected abstract void StringContainsSql();

        /// <summary>
        /// BooleanValue = OLD.StringField.EndsWith("abc")
        /// </summary>
        protected Expression<Func<SourceEntity, DestinationEntity>> EndsWithStringValueExpression = sourceEntity => new DestinationEntity
        {
            BooleanValue = sourceEntity.StringField.EndsWith("abc")
        };

        [Fact]
        protected abstract void StringEndsWithSql();

        /// <summary>
        /// BooleanValue = string.IsNullOrEmpty(OLD.StringField)
        /// </summary>
        protected Expression<Func<SourceEntity, DestinationEntity>> IsNullOrEmptyStringValueExpression = sourceEntity => new DestinationEntity
        {
            BooleanValue = string.IsNullOrEmpty(sourceEntity.StringField)
        };

        [Fact]
        protected abstract void StringIsNullOrEmptySql();
        
        /// <summary>
        /// BooleanValue = string.IsNullOrEmpty(OLD.StringField)
        /// </summary>
        protected Expression<Func<SourceEntity, DestinationEntity>> CoalesceStringExpression = sourceEntity => new DestinationEntity
        {
            StringField = sourceEntity.StringField ?? "John"
        };

        [Fact]
        protected abstract void CoalesceStringSql();
    }
}