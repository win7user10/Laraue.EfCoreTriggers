using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.MySql.Extensions;
using Laraue.EfCoreTriggers.Tests.Entities;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.Linq2Triggers.Core.Converters.MethodCall.String;
using Laraue.Linq2Triggers.Core.Extensions;
using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.TriggerBuilders.Actions;
using Laraue.Linq2Triggers.Core.TriggerBuilders.TableRefs;
using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.Tests.Tests
{
    [UnitTest]
    public class SetupProviderConverterTests
    {
        private readonly ModelBuilder _modelBuilder = new ();

        public SetupProviderConverterTests()
        {
            _modelBuilder.Entity<Transaction>()
                .Property<string>("Description");
        }

        [Fact]
        public virtual void Provider_ShouldUseCustomConverter_WhenItProvidedForFunction()
        {
            var provider = Helper.GetTriggerActionFactory(_modelBuilder.Model.FinalizeModel(), services =>
            {
                services.AddMySqlServices();
                
                services.AddMethodCallConverter<CustomStringToUpperVisitor>();
            });

            Expression<Func<NewTableRef<Transaction>, bool>> condition = tableRefs =>
                tableRefs.New.Description.ToUpper() == "ABC";
            var action = new TriggerCondition(condition);
            
            var sql = provider.Visit(action, new VisitedMembers());
            
            Assert.Equal("test = 'ABC'", sql);
        }

        private class CustomStringToUpperVisitor : BaseStringVisitor
        {
            public CustomStringToUpperVisitor(IExpressionVisitorFactory visitorFactory) 
                : base(visitorFactory)
            {
            }

            protected override string MethodName => nameof(string.ToUpper);
            
            public override SqlBuilder Visit(MethodCallExpression expression, VisitedMembers visitedMembers)
            {
                return SqlBuilder.FromString("test");
            }
        }
    }
}
