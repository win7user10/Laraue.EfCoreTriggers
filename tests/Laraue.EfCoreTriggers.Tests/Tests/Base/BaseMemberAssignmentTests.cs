﻿using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;
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
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> SetEnumValueExpression =
            tableRefs => new DestinationEntity
            {
                EnumValue = tableRefs.New.EnumValue
            };

        [Fact]
        public abstract void EnumValueSql();

        /// <summary>
        /// DecimalValue = Old.DecimalValue + 3
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> AddDecimalValueExpression =
            tableRefs => new DestinationEntity
            {
                DecimalValue = tableRefs.New.DecimalValue + 3
            };

        [Fact]
        public abstract void DecimalAddSql();

        /// <summary>
        /// DoubleValue = Old.DoubleValue + 3
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> SubDoubleValueExpression =
            tableRefs => new DestinationEntity
            {
                DoubleValue = tableRefs.New.DoubleValue + 3
            };

        [Fact]
        public abstract void DoubleSubSql();

        /// <summary>
        /// NEW.DoubleValue = Old.DoubleValue + 3
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> MultiplyIntValueExpression =
            tableRefs => new DestinationEntity
            {
                IntValue = tableRefs.New.IntValue * 2
            };

        [Fact]
        public abstract void IntMultiplySql();

        /// <summary>
        /// BooleanValue = true
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> SetBooleanValueExpression =
            tableRefs => new DestinationEntity
            {
                BooleanValue = !tableRefs.New.BooleanValue
            };

        [Fact]
        public abstract void BooleanValueSql();

        /// <summary>
        /// GuidValue = xxxxxxxx-xxxxxx-xxxxxx-xxxxxxxx
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> SetNewGuidValueExpression =
            tableRefs => new DestinationEntity
            {
                GuidValue = Guid.NewGuid()
            };

        [Fact]
        public abstract void NewGuid();
        
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> SetCharVariableExpression =
            tableRefs => new DestinationEntity
            {
                CharValue = tableRefs.New.CharValue
            };
        
        [Fact]
        public abstract void CharVariableSql();
        
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> SetCharValueExpression =
            tableRefs => new DestinationEntity
            {
                CharValue = 'a'
            };
        
        [Fact]
        public abstract void CharValueSql();
        
        /// <summary>
        /// DateTimeOffsetValue = DateTimeOffset.UtcNow
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> SetDateTimeOffsetUtcNowExpression =
            tableRefs => new DestinationEntity
            {
                DateTimeOffsetValue = DateTimeOffset.UtcNow,
            };
        
        [Fact]
        public abstract void DateTimeOffsetUtcNowSql();
        
        /// <summary>
        /// DateTimeOffsetValue = DateTimeOffset.Now
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> SetDateTimeOffsetNowExpression =
            tableRefs => new DestinationEntity
            {
                DateTimeOffsetValue = DateTimeOffset.Now,
            };
        
        [Fact]
        public abstract void DateTimeOffsetNowSql();
        
        /// <summary>
        /// DateTime = new DateTime()
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> SetNewDateTimeExpression =
            tableRefs => new DestinationEntity
            {
                DateTimeValue = new DateTime()
            };

        [Fact]
        public abstract void NewDateTimeSql();
        
        /// <summary>
        /// DateTime = new DateTime()
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> SetNewDateTimeOffsetExpression =
            tableRefs => new DestinationEntity
            {
                DateTimeOffsetValue = new DateTimeOffset()
            };

        [Fact]
        public abstract void NewDateTimeOffsetSql();
    }
}