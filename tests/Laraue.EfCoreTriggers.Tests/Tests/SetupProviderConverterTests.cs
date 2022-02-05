using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.OnInsert;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;
using Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;
using Laraue.EfCoreTriggers.MySql;
using Laraue.EfCoreTriggers.Tests.Entities;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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
            var provider = Helper.GetTriggerActionFactory(_modelBuilder.Model, converters =>
            {
                converters.AddMethodCallConverter<CustomStringToUpperVisitor>();
            });

            var action = new OnInsertTriggerCondition<Transaction>(t => t.Description.ToUpper() == "ABC");
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
            
            public override SqlBuilder Visit(MethodCallExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
            {
                return SqlBuilder.FromString("test");
            }
        }
    }
}
