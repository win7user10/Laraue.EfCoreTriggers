using System;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.OnInsert;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;
using Laraue.EfCoreTriggers.MySql;
using Laraue.EfCoreTriggers.MySql.Extensions;
using Laraue.EfCoreTriggers.Tests.Entities;
using Laraue.EfCoreTriggers.Tests.Enums;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.Tests.Tests
{
    [UnitTest]
	public class ConditionGeneratingTests
    {
        private readonly ITriggerActionVisitorFactory _provider;

        public ConditionGeneratingTests()
        {
            var modelBuilder = new ModelBuilder();
            modelBuilder.Entity<User>()
                .Property<UserRole>("Role");

            _provider = Helper.GetTriggerActionFactory(modelBuilder.Model, collection => collection.AddMySqlServices());
        }

        [Fact]
        public virtual void CastingToSameTypeShouldBeIgnored()
        {
            var action = new OnInsertTriggerCondition<User>(user => user.Role > UserRole.Admin);
            var sql = _provider.Visit(action, new VisitedMembers());
            Assert.Equal("NEW.Role > 999", sql);
        }

        [Fact]
        public virtual void CastingToAnotherTypeShouldBeTranslated()
        {
            var action = new OnInsertTriggerCondition<User>(user => (decimal)user.Role > 50m);
            var sql = _provider.Visit(action, new VisitedMembers());
            Assert.Equal("CAST(NEW.Role AS DECIMAL) > 50", sql);
        }
    }
}
