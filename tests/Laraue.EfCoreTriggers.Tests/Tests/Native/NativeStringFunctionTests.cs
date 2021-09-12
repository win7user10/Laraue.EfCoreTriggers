using Laraue.EfCoreTriggers.Tests.Tests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        protected NativeStringFunctionTests(IContextOptionsFactory<DynamicDbContext> contextOptionsFactory)
        {
            ContextOptionsFactory = contextOptionsFactory;
        }
        protected override void StringConcatSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(ConcatStringExpression, new SourceEntity
            {
                StringField = "daw "
            });
            Assert.Equal("daw abc", insertedEntity.StringField);
        }

        protected override void StringLowerSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(StringToLowerExpression, new SourceEntity
            {
                StringField = "AbS"
            });
            Assert.Equal("abs", insertedEntity.StringField);
        }

        protected override void StringUpperSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(StringToUpperExpression, new SourceEntity
            {
                StringField = "AbS"
            });
            Assert.Equal("ABS", insertedEntity.StringField);
        }

        protected override void StringTrimSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(TrimStringValueExpression, new SourceEntity
            {
                StringField = "              AbS"
            });
            Assert.Equal("AbS", insertedEntity.StringField);
        }

        protected override void StringContainsSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(ContainsStringValueExpression, new SourceEntity
            {
                StringField = "    abs    abc      AbS"
            });
            Assert.Equal(true, insertedEntity.BooleanValue);
            insertedEntity = ContextOptionsFactory.CheckTrigger(ContainsStringValueExpression, new SourceEntity
            {
                StringField = "   dknjp;;s jd"
            });
            Assert.Equal(false, insertedEntity.BooleanValue);
        }

        protected override void StringEndsWithSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(EndsWithStringValueExpression, new SourceEntity
            {
                StringField = "  abs abcid abc"
            });
            Assert.Equal(true, insertedEntity.BooleanValue);
            insertedEntity = ContextOptionsFactory.CheckTrigger(EndsWithStringValueExpression, new SourceEntity
            {
                StringField = "  abc dknjp;;s jd"
            });
            Assert.Equal(false, insertedEntity.BooleanValue);
        }

        protected override void StringIsNullOrEmptySql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(IsNullOrEmptyStringValueExpression, new SourceEntity
            {
                StringField = "  abc dknjp;;s jd"
            });
            Assert.Equal(false, insertedEntity.BooleanValue);
            insertedEntity = ContextOptionsFactory.CheckTrigger(IsNullOrEmptyStringValueExpression, new SourceEntity
            {
                StringField = ""
            });
            Assert.Equal(true, insertedEntity.BooleanValue);
            insertedEntity = ContextOptionsFactory.CheckTrigger(IsNullOrEmptyStringValueExpression, new SourceEntity
            {
                StringField = null
            });
            Assert.Equal(true, insertedEntity.BooleanValue);
        }
    }
}
