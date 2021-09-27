using System;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.OnInsert;
using Laraue.EfCoreTriggers.MySql;
using Laraue.EfCoreTriggers.Tests.Entities;
using Laraue.EfCoreTriggers.Tests.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Xunit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.Tests.Tests
{
    [UnitTest]
    public class InvalidOperationTests
    {
        private readonly ITriggerProvider _provider;

        public InvalidOperationTests()
        {
            var modelBuilder = new ModelBuilder();
            IModel model = modelBuilder.Model;
            _provider = new MySqlProvider(model);
        }
        
        [Fact]
        public virtual void Entity_NotExistsInDbContext_ShouldThrowException()
        {
            var action = new OnInsertTriggerCondition<User>(user => user.Role == UserRole.Admin);
            var ex = Assert.Throws<InvalidOperationException>(() => action.BuildSql(_provider));
            Assert.Equal("DbSet<User> should be added to the DbContext", ex.Message);
        }
    }
}