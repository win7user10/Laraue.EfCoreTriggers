using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Actions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;
using Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors;
using Laraue.EfCoreTriggers.MySql.Extensions;
using Laraue.EfCoreTriggers.Tests.Entities;
using Laraue.EfCoreTriggers.Tests.Enums;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.Tests.Tests
{
    [UnitTest]
    public sealed class InvalidOperationTests
    {
        private readonly ITriggerActionVisitorFactory _provider;

        public InvalidOperationTests()
        {
            _provider = Helper.GetService<ITriggerActionVisitorFactory>(new ModelBuilder(), collection => collection.AddMySqlServices());
        }
        
        [Fact]
        public void Entity_NotExistsInDbContext_ShouldThrowException()
        {
            Expression<Func<NewTableRef<User>, bool>> condition = tableRefs => tableRefs.New.Role == UserRole.Admin;
            var action = new TriggerCondition(condition);
            
            var ex = Assert.Throws<InvalidOperationException>(() => _provider.Visit(action, new VisitedMembers()));
            
            Assert.Equal("DbSet<Laraue.EfCoreTriggers.Tests.Entities.User> should be added to the DbContext", ex.Message);
        }
    }
}