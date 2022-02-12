using System;
using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.OnInsert;
using Laraue.EfCoreTriggers.MySql;
using Laraue.EfCoreTriggers.MySql.Extensions;
using Laraue.EfCoreTriggers.Tests.Entities;
using Laraue.EfCoreTriggers.Tests.Enums;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Xunit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.Tests.Tests
{
    [UnitTest]
    public class InvalidOperationTests
    {
        private readonly ITriggerActionVisitorFactory _provider;

        public InvalidOperationTests()
        {
            _provider = Helper.GetService<ITriggerActionVisitorFactory>(new ModelBuilder(), collection => collection.AddMySqlServices());
        }
        
        [Fact]
        public virtual void Entity_NotExistsInDbContext_ShouldThrowException()
        {
            var action = new OnInsertTriggerCondition<User>(user => user.Role == UserRole.Admin);
            var ex = Assert.Throws<InvalidOperationException>(() => _provider.Visit(action, new VisitedMembers()));
            Assert.Equal("DbSet<User> should be added to the DbContext", ex.Message);
        }
    }
}