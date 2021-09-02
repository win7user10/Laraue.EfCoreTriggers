using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Base;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.Tests.Tests.Native
{
    [IntegrationTest]
    public abstract class NativeMathFunctionTests : BaseMathFunctionsTests
    {
        protected IContextOptionsFactory<DynamicDbContext> ContextOptionsFactory { get; }

        protected NativeMathFunctionTests(IContextOptionsFactory<DynamicDbContext> contextOptionsFactory)
        {
            ContextOptionsFactory = contextOptionsFactory;
        }


        public static DestinationEntity ExecuteTest(IContextOptionsFactory<DynamicDbContext> contextOptionsFactory, Expression<Func<SourceEntity,DestinationEntity>> triggerExpression, SourceEntity source)
        {
            using var dbContext = contextOptionsFactory.GetDbContext(triggerExpression);

            dbContext.SourceEntities.Add(source);
            dbContext.SaveChanges();

            return Assert.Single(dbContext.DestinationEntities);
        }

        public override void MathAbsDecimalSql()
        {
            var insertedEntity = ExecuteTest(ContextOptionsFactory, MathAbsDecimalValueExpression, new SourceEntity
            {
                DecimalValue = -2.04M,
            });

            Assert.Equal(2.04M, insertedEntity.DecimalValue);
        }

        public override void MathAcosSql()
        {
            var insertedEntity = ExecuteTest(ContextOptionsFactory, MathAcosDoubleValueExpression, new SourceEntity
            {
                DoubleValue = 1,
            });
            
            Assert.Equal(0, insertedEntity.DoubleValue);
        }

        public override void MathAsinSql()
        {
            var insertedEntity = ExecuteTest(ContextOptionsFactory, MathAsinDoubleValueExpression, new SourceEntity
            {
                DoubleValue = 0,
            });

            Assert.Equal(0, insertedEntity.DoubleValue);
        }

        public override void MathAtanSql()
        {
            var insertedEntity = ExecuteTest(ContextOptionsFactory, MathAtanDoubleValueExpression, new SourceEntity
            {
                DoubleValue = 0,
            });

            Assert.Equal(0, insertedEntity.DoubleValue);
        }

        public override void MathAtan2Sql()
        {
            var insertedEntity = ExecuteTest(ContextOptionsFactory, MathAtan2DoubleValueExpression, new SourceEntity
            {
                DoubleValue = 1,
            });

            Assert.Equal(0.7853, Math.Round(insertedEntity.DoubleValue.Value, 4, MidpointRounding.ToNegativeInfinity));
        }

        public override void MathCeilingDoubleSql()
        {
            throw new System.NotImplementedException();
        }

        public override void MathCosSql()
        {
            throw new System.NotImplementedException();
        }

        public override void MathExpSql()
        {
            throw new System.NotImplementedException();
        }

        public override void MathFloorDoubleSql()
        {
            throw new System.NotImplementedException();
        }
    }
}