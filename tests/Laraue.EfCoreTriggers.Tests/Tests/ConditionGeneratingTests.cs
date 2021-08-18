using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.OnInsert;
using Laraue.EfCoreTriggers.MySql;
using Laraue.EfCoreTriggers.Tests.Entities;
using Laraue.EfCoreTriggers.Tests.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Xunit;

namespace Laraue.EfCoreTriggers.Tests.Tests
{
	public class ConditionGeneratingTests
    {
        private readonly ITriggerProvider _provider;
        private readonly IModel _model;

        public ConditionGeneratingTests()
        {
            var modelBuilder = new ModelBuilder();
            modelBuilder.Entity<User>()
                .Property<UserRole>("Role");

            _model = modelBuilder.Model;
            _provider = new MySqlProvider(_model);
        }

        [Fact]
        public virtual void CastingToSameTypeShouldBeIgnored()
        {
            var action = new OnInsertTriggerCondition<User>(user => user.Role > UserRole.Admin);
            var sql = action.BuildSql(_provider);
            Assert.Equal("NEW.Role > 999", sql);
        }

        [Fact]
        public virtual void CastingToAnotherTypeShouldBeTranslated()
        {
            var action = new OnInsertTriggerCondition<User>(user => (decimal)user.Role > 50m);
            var sql = action.BuildSql(_provider);
            Assert.Equal("CAST(NEW.Role AS DECIMAL) > 50", sql);
        }
    }
}
