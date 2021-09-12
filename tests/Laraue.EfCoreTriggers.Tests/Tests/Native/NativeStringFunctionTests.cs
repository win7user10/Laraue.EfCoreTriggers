using System;
using Laraue.EfCoreTriggers.Tests.Tests.Base;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Xunit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.Tests.Tests.Native
{
    [IntegrationTest]
    [Collection("IntegrationTests")]
    public abstract class NativeStringFunctionTests : BaseStringFunctionsTests
    {
        protected IContextOptionsFactory<DynamicDbContext> ContextOptionsFactory { get; }
        protected Action<DynamicDbContext> SetupDbContext { get; }

        protected NativeStringFunctionTests(IContextOptionsFactory<DynamicDbContext> contextOptionsFactory, Action<DynamicDbContext> setupDbContext = null)
        {
            ContextOptionsFactory = contextOptionsFactory;
            SetupDbContext = setupDbContext;
        }

        protected override void StringConcatSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(ConcatStringExpression, SetupDbContext, new SourceEntity
            {
                StringField = "daw "
            });
            Assert.Equal("daw abc", insertedEntity.StringField);
        }

        protected override void StringLowerSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(StringToLowerExpression, SetupDbContext, new SourceEntity
            {
                StringField = "AbS"
            });
            Assert.Equal("abs", insertedEntity.StringField);
        }

        protected override void StringUpperSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(StringToUpperExpression, SetupDbContext, new SourceEntity
            {
                StringField = "AbS"
            });
            Assert.Equal("ABS", insertedEntity.StringField);
        }

        protected override void StringTrimSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(TrimStringValueExpression, SetupDbContext, new SourceEntity
            {
                StringField = "              AbS"
            });
            Assert.Equal("AbS", insertedEntity.StringField);
        }

        protected override void StringContainsSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(ContainsStringValueExpression, SetupDbContext, new SourceEntity
            {
                StringField = "    abs    abc      AbS"
            });
            Assert.Equal(true, insertedEntity.BooleanValue);
            insertedEntity = ContextOptionsFactory.CheckTrigger(ContainsStringValueExpression, SetupDbContext, new SourceEntity
            {
                StringField = "   dknjp;;s jd"
            });
            Assert.Equal(false, insertedEntity.BooleanValue);
        }

        protected override void StringEndsWithSql()
        {
            var sourceEntities = new[]
            {
                new SourceEntity { StringField = "  abs abcid abc" },
                new SourceEntity { StringField = "  abc dknjp;;s jd" }
            };

            var insertedEntities = ContextOptionsFactory.CheckTrigger(EndsWithStringValueExpression, SetupDbContext, sourceEntities);


            Assert.Equal(2, insertedEntities.Length);
            Assert.True(insertedEntities[0].BooleanValue);
            Assert.False(insertedEntities[1].BooleanValue);
        }

        protected override void StringIsNullOrEmptySql()
        {
            var sourceEntities = new[]
            {
                new SourceEntity { StringField = "  abc dknjp;;s jd" },
                new SourceEntity { StringField = "" },
                new SourceEntity { StringField = null }
            };

            var insertedEntities = ContextOptionsFactory.CheckTrigger(IsNullOrEmptyStringValueExpression, SetupDbContext, sourceEntities);

            Assert.Equal(3, insertedEntities.Length);
            Assert.False(insertedEntities[0].BooleanValue);
            Assert.True(insertedEntities[1].BooleanValue);
            Assert.True(insertedEntities[2].BooleanValue);
        }
    }
}
