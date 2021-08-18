using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.OnInsert;
using Laraue.EfCoreTriggers.MySql;
using Laraue.EfCoreTriggers.Tests.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Xunit;

namespace Laraue.EfCoreTriggers.Tests.Tests
{
	public class SetupProviderConverterTests
    {
        private readonly ITriggerProvider _provider;

        public SetupProviderConverterTests()
        {
            var modelBuilder = new ModelBuilder();
            modelBuilder.Entity<Transaction>()
                .Property<string>("Description");

            IModel model = modelBuilder.Model;
            _provider = new MySqlProvider(model);
        }

        [Fact]
        public virtual void Provider_ShouldUseCustomConverter_WhenItProvidedForFunction()
        {
            _provider.Converters.ExpressionCallConverters.Push(new CustomStringToUpperConverter());

            var action = new OnInsertTriggerCondition<Transaction>(t => t.Description.ToUpper() == "ABC");
            var sql = action.BuildSql(_provider);
            Assert.Equal("test = 'ABC'", sql);
        }

        private class CustomStringToUpperConverter : BaseStringConverter
        {
            public override SqlBuilder BuildSql(BaseExpressionProvider provider, MethodCallExpression expression, Dictionary<string, ArgumentType> argumentTypes)
            {
                return new ("test");
            }

            public override string MethodName => nameof(string.ToUpper);
        }
    }
}
