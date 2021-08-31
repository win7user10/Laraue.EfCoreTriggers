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

        public override void MathAbsDecimalSql()
        {
            using var dbContext = ContextOptionsFactory.GetDbContext(MathAbsDecimalValueExpression);

            dbContext.SourceEntities.Add(new SourceEntity
            {
                DecimalValue = -2.04M,
            });
            dbContext.SaveChanges();

            var insertedEntity = Assert.Single(dbContext.DestinationEntities);
            Assert.Equal(2.04M, insertedEntity.DecimalValue);
        }

        public override void MathAcosSql()
        {
            throw new System.NotImplementedException();
        }

        public override void MathAsinSql()
        {
            throw new System.NotImplementedException();
        }

        public override void MathAtanSql()
        {
            throw new System.NotImplementedException();
        }

        public override void MathAtan2Sql()
        {
            throw new System.NotImplementedException();
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