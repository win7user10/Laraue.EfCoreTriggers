using System;
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
                GuidValue = new Guid()
            });
            Assert.NotEqual(default, insertedEntity.GuidValue);
        }
    }
}
