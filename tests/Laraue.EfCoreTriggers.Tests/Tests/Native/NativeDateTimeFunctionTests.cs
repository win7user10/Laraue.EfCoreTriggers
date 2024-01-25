using System;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Base;
using Xunit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.Tests.Tests.Native
{
    [IntegrationTest]
    public abstract class NativeDateTimeFunctionTests : BaseDateTimeFunctionsTests
    {
        protected IContextOptionsFactory<DynamicDbContext> ContextOptionsFactory { get; }
        protected Action<DynamicDbContext> SetupDbContext { get; }

        protected NativeDateTimeFunctionTests(IContextOptionsFactory<DynamicDbContext> contextOptionsFactory, Action<DynamicDbContext> setupDbContext = null)
        {
            ContextOptionsFactory = contextOptionsFactory;
            SetupDbContext = setupDbContext;
        }

        /// <summary>
        /// Returns true if data stores only in UTC in the DB.
        /// </summary>
        protected abstract bool DateReturnsOnlyInUtc { get; }

        public override void DateTimeUtcNowSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(
                DateTimeUtcNowExpression,
                SetupDbContext,
                null,
                new SourceEntity());

            Assert.Equal(
                DateTime.UtcNow,
                insertedEntity.DateTimeValue.GetValueOrDefault(),
                new TimeSpan(0, 1, 0));
        }

        public override void DateTimeNowSql()
        {
            var insertedEntity = ContextOptionsFactory.CheckTrigger(
                DateTimeNowExpression,
                SetupDbContext,
                null,
                new SourceEntity());

            var localTime = DateTime.Now;
            var dbDate = insertedEntity.DateTimeValue.GetValueOrDefault();
            
            // Some databases stores DateTime in UTC as default. Convert such dates to local dt.
            // Assumes that db works in the same timezone as the test running machine.
            if (DateReturnsOnlyInUtc)
            {
                dbDate = TimeZoneInfo.ConvertTimeFromUtc(dbDate, TimeZoneInfo.Local);
            }
            
            Assert.Equal(dbDate, localTime, new TimeSpan(0, 1, 0));
        }
    }
}