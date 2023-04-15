using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Actions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;
using Laraue.EfCoreTriggers.MySql.Extensions;
using Laraue.EfCoreTriggers.Tests.Entities;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
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
