using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Base;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.Tests.Tests.Native
{
    [IntegrationTest]
    [Collection("IntegrationTests")]
    public abstract class NativeMemberAssignmentFunctionsTests : BaseMemberAssignmentTests
    {
        protected IContextOptionsFactory<DynamicDbContext> ContextOptionsFactory { get; }
        protected Action<DynamicDbContext> SetupDbContext { get; }
        protected Action<ModelBuilder> SetupModelBuilder { get; }

        protected NativeMemberAssignmentFunctionsTests(
            IContextOptionsFactory<DynamicDbContext> contextOptionsFactory, 
            Action<DynamicDbContext> setupDbContext = null,
            Action<ModelBuilder> setupModelBuilder = null)
        {
            ContextOptionsFactory = contextOptionsFactory;
            SetupDbContext = setupDbContext;
            SetupModelBuilder = setupModelBuilder;
        }

        public override void EnumValueSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(SetEnumValueExpression, SetupDbContext, SetupModelBuilder, new SourceEntity
            {
                EnumValue = EnumValue.Value1
            });
            Assert.Equal(EnumValue.Value1, insertedEntity.EnumValue);
        }

        public override void DecimalAddSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(AddDecimalValueExpression, SetupDbContext, SetupModelBuilder, new SourceEntity
            {
                DecimalValue = 10.3M
            });
            Assert.Equal(13.3M, insertedEntity.DecimalValue);
        }

        public override void DoubleSubSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(SubDoubleValueExpression, SetupDbContext, SetupModelBuilder, new SourceEntity
            {
                DoubleValue = 10.3
            });
            Assert.Equal(13.3, insertedEntity.DoubleValue);
        }

        public override void IntMultiplySql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(MultiplyIntValueExpression, SetupDbContext, SetupModelBuilder, new SourceEntity
            {
                IntValue = 12
            });
            Assert.Equal(24, insertedEntity.IntValue);
        }

        public override void BooleanValueSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(SetBooleanValueExpression, SetupDbContext, SetupModelBuilder, new SourceEntity
            {
                BooleanValue = true
            });
            Assert.Equal(false, insertedEntity.BooleanValue);
        }

        public override void NewGuid()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(SetNewGuidValueExpression, SetupDbContext, SetupModelBuilder, new SourceEntity
            {
                GuidValue = Guid.NewGuid()
            });
            Assert.NotEqual(default, insertedEntity.GuidValue);
        }

        public override void CharVariableSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(SetCharVariableExpression, SetupDbContext, SetupModelBuilder, new SourceEntity
            {
                CharValue = 'a'
            });
            
            Assert.Equal('a', insertedEntity.CharValue);
        }
        
        [Fact]
        public void NumberEnumSql()
        {
            Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> setEnumVariableExpression = tableRefs
                => new DestinationEntity
                {
                    EnumValue = EnumValue.Value2,
                };

            var insertedEntity = ContextOptionsFactory.CheckTrigger(
                setEnumVariableExpression,
                SetupDbContext,
                SetupModelBuilder,
                new SourceEntity
                {
                    CharValue = 'a'
                });
            
            Assert.Equal(EnumValue.Value2, insertedEntity.EnumValue);
        }
        
        [Fact]
        public void StringEnumSql()
        {
            Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> setEnumVariableExpression = tableRefs
                => new DestinationEntity
                {
                    EnumValue = tableRefs.New.EnumValue,
                };

            Action<ModelBuilder> setupModelBuilder = builder =>
            {
                builder.Entity<SourceEntity>()
                    .Property(x => x.EnumValue)
                    .HasColumnType("varchar(100)")
                    .HasConversion<string>();

                builder.Entity<DestinationEntity>()
                    .Property(x => x.EnumValue)
                    .HasColumnType("varchar(100)")
                    .HasConversion<string>();
            };
            
            setupModelBuilder += SetupModelBuilder;

            var insertedEntity = ContextOptionsFactory.CheckTrigger(setEnumVariableExpression, SetupDbContext, setupModelBuilder, new SourceEntity
            {
                EnumValue = EnumValue.Value2
            });
            
            Assert.Equal(EnumValue.Value2, insertedEntity.EnumValue);
        }

        public override void CharValueSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(SetCharValueExpression, SetupDbContext, SetupModelBuilder, new SourceEntity());
            
            Assert.Equal('a', insertedEntity.CharValue);
        }

        public override void DateTimeOffsetNowSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(SetDateTimeOffsetNowExpression, SetupDbContext, SetupModelBuilder, new SourceEntity());
            
            Assert.Equal(DateTimeOffset.Now.Date, insertedEntity.DateTimeOffsetValue.GetValueOrDefault().Date, TimeSpan.FromDays(1));
        }

        public override void DateTimeOffsetUtcNowSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(SetDateTimeOffsetUtcNowExpression, SetupDbContext, SetupModelBuilder, new SourceEntity());
            
            Assert.Equal(DateTimeOffset.UtcNow.Date, insertedEntity.DateTimeOffsetValue.GetValueOrDefault().Date, TimeSpan.FromDays(1));
        }

        public override void NewDateTimeSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(SetNewDateTimeExpression, SetupDbContext, SetupModelBuilder, new SourceEntity());
            
            // Just a smoke, all DB providers have different Min Dates
            var dbMinDate = insertedEntity.DateTimeValue.GetValueOrDefault();
            Assert.True(dbMinDate < new DateTime(2000, 01, 01));
        }

        public override void NewDateTimeOffsetSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(SetNewDateTimeOffsetExpression, SetupDbContext, SetupModelBuilder, new SourceEntity());
            
            // Just a smoke, all DB providers have different Min DateTime Offsets
            var dbMinDateOffset = insertedEntity.DateTimeOffsetValue.GetValueOrDefault();
            Assert.True(dbMinDateOffset < new DateTime(2000, 01, 01));
        }
    }
}
