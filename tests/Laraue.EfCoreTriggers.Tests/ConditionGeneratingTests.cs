using Laraue.EfCoreTriggers.Common.Builders.Providers;
using Laraue.EfCoreTriggers.Common.Builders.Triggers.OnInsert;
using Laraue.EfCoreTriggers.Tests.Entities;
using Laraue.EfCoreTriggers.Tests.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using Xunit;

namespace Laraue.EfCoreTriggers.Tests
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
        public virtual void EnumValueConvertSql()
        {
            var action = new OnInsertTriggerCondition<User>(user => user.Role > UserRole.Usual);
            var sql = action.BuildSql(_provider);
            Assert.Equal("CAST (NEW.Role AS int) > 0", sql);
        }
    }
}
