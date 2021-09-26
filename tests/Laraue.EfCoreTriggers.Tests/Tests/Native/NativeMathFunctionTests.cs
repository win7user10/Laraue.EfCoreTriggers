using System;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Base;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.Tests.Tests.Native
{
    [IntegrationTest]
    public abstract class NativeMathFunctionTests : BaseMathFunctionsTests
    {
        protected IContextOptionsFactory<DynamicDbContext> ContextOptionsFactory { get; }
        protected Action<DynamicDbContext> SetupDbContext { get; }
        protected Action<ModelBuilder> SetupModelBuilder { get; }

        protected NativeMathFunctionTests(IContextOptionsFactory<DynamicDbContext> contextOptionsFactory, Action<DynamicDbContext> setupDbContext = null)
        {
            ContextOptionsFactory = contextOptionsFactory;
            SetupDbContext = setupDbContext;
        }

        public static double CustomMathRound(double value)
        {
            return Math.Round(value, 4, MidpointRounding.ToNegativeInfinity);
        }

        public override void MathAbsDecimalSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(MathAbsDecimalValueExpression, SetupDbContext, SetupModelBuilder,  new SourceEntity
            {
                DecimalValue = -2.04M,
            });

            Assert.Equal(2.04M, insertedEntity.DecimalValue);
        }

        public override void MathAcosSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(MathAcosDoubleValueExpression, SetupDbContext, SetupModelBuilder, new SourceEntity
            {
                DoubleValue = 1,
            });
            
            Assert.Equal(0, insertedEntity.DoubleValue);
        }

        public override void MathAsinSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(MathAsinDoubleValueExpression, SetupDbContext, SetupModelBuilder, new SourceEntity
            {
                DoubleValue = 0,
            });

            Assert.Equal(0, insertedEntity.DoubleValue);
        }

        public override void MathAtanSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(MathAtanDoubleValueExpression, SetupDbContext, SetupModelBuilder, new SourceEntity
            {
                DoubleValue = 0,
            });

            Assert.Equal(0, insertedEntity.DoubleValue);
        }

        public override void MathAtan2Sql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(MathAtan2DoubleValueExpression, SetupDbContext, SetupModelBuilder, new SourceEntity
            {
                DoubleValue = 1,
            });

            Assert.Equal(0.7853, CustomMathRound(insertedEntity.DoubleValue.Value));
        }

        public override void MathCeilingDoubleSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(MathCeilingDoubleValueExpression, SetupDbContext, SetupModelBuilder, new SourceEntity
            {
                DoubleValue = -1.36,
            });

            Assert.Equal(-1, insertedEntity.DoubleValue);
        }

        public override void MathCosSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(MathCosDoubleValueExpression, SetupDbContext, SetupModelBuilder, new SourceEntity
            {
                DoubleValue = -1,
            });

            Assert.Equal(0.5403, CustomMathRound(insertedEntity.DoubleValue.Value));
        }

        public override void MathExpSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(MathExpDoubleValueExpression, SetupDbContext, SetupModelBuilder, new SourceEntity
            {
                DoubleValue = 1.2,
            });

            Assert.Equal(3.3201, CustomMathRound(insertedEntity.DoubleValue.Value));
        }

        public override void MathFloorDoubleSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(MathFloorDoubleValueExpression, SetupDbContext, SetupModelBuilder, new SourceEntity
            {
                DoubleValue = -1.2,
            });

            Assert.Equal(-2, insertedEntity.DoubleValue);
        }
    }
}