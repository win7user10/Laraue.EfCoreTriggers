using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Xunit;

namespace Laraue.EfCoreTriggers.Tests.Tests.Base
{
    /// <summary>
    /// Tests of translating <see cref="MemberAssignment"/> to SQL code.
    /// </summary>
    public abstract class BaseMemberAssignmentTests
    {
        /// <summary>
        /// EnumValue = Old.EnumValue
        /// </summary>
        protected Expression<Func<SourceEntity, DestinationEntity>> SetEnumValueExpression = sourceEntity => new DestinationEntity
        {
            EnumValue = sourceEntity.EnumValue
        };

        [Fact]
        public abstract void EnumValueSql();

        /// <summary>
        /// DecimalValue = Old.DecimalValue + 3
        /// </summary>
        protected Expression<Func<SourceEntity, DestinationEntity>> AddDecimalValueExpression = sourceEntity => new DestinationEntity
        {
            DecimalValue = sourceEntity.DecimalValue + 3
        };

        [Fact]
        public abstract void DecimalAddSql();

        /// <summary>
        /// DoubleValue = Old.DoubleValue + 3
        /// </summary>
        protected Expression<Func<SourceEntity, DestinationEntity>> SubDoubleValueExpression = sourceEntity => new DestinationEntity
        {
            DoubleValue = sourceEntity.DoubleValue + 3
        };

        [Fact]
        public abstract void DoubleSubSql();

        /// <summary>
        /// NEW.DoubleValue = Old.DoubleValue + 3
        /// </summary>
        protected Expression<Func<SourceEntity, DestinationEntity>> MultiplyIntValueExpression = sourceEntity => new DestinationEntity
        {
            IntValue = sourceEntity.IntValue * 2
        };

        [Fact]
        public abstract void IntMultiplySql();

        /// <summary>
        /// BooleanValue = true
        /// </summary>
        protected Expression<Func<SourceEntity, DestinationEntity>> SetBooleanValueExpression = sourceEntity => new DestinationEntity
        {
            BooleanValue = !sourceEntity.BooleanValue
        };

        [Fact]
        public abstract void BooleanValueSql();

        /// <summary>
        /// GuidValue = xxxxxxxx-xxxxxx-xxxxxx-xxxxxxxx
        /// </summary>
        protected Expression<Func<SourceEntity, DestinationEntity>> SetNewGuidValueExpression = sourceEntity => new DestinationEntity
        {
            GuidValue = new Guid()
        };

        [Fact]
        public abstract void NewGuid();
        
        protected Expression<Func<SourceEntity, DestinationEntity>> SetCharVariableExpression = sourceEntity => new DestinationEntity
        {
            CharValue = sourceEntity.CharValue
        };
        
        [Fact]
        public abstract void CharVariableSql();
        
        protected Expression<Func<SourceEntity, DestinationEntity>> SetCharValueExpression = sourceEntity => new DestinationEntity
        {
            CharValue = 'a'
        };
        
        [Fact]
        public abstract void CharValueSql();
        
        /// <summary>
        /// DateTimeOffsetValue = CURRENT_DATE()
        /// </summary>
        protected Expression<Func<SourceEntity, DestinationEntity>> SetNewDateOffsetValueExpression = sourceEntity => new DestinationEntity
        {
            DateTimeOffsetValue = new DateTimeOffset(),
        };
        
        [Fact]
        public abstract void DateTimeOffsetValueSql();
    }
}