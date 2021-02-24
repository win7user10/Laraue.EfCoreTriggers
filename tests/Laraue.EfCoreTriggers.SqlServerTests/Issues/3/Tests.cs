using Laraue.EfCoreTriggers.Common.Builders.Providers;
using Laraue.EfCoreTriggers.Common.Builders.Triggers.OnInsert;
using Laraue.EfCoreTriggers.Extensions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Laraue.EfCoreTriggers.SqlServerTests.Issues._3
{
	public class Tests
	{
		private readonly ITriggerProvider _provider;

		public Tests()
		{
			var context = new TestDbContext(new DbContextOptionsBuilder<TestDbContext>()
				.UseSqlServer()
				.UseTriggers()
				.Options);

			_provider = new SqlServerProvider(context.Model); 
		}

		[Fact]
		public virtual void ShouldBeGeneratedCorrectSql()
		{
			var trigger = new OnInsertTrigger<SalesCategory>(TriggerTime.After)
				.Action(action => action
					.Condition(f => f.Status == EntityStatus.New)
					.Update<SalesCategory>((a, b) => a.Id == b.Id, (a, b) => new SalesCategory() { Status = EntityStatus.Draft }));

			var sql = trigger.BuildSql(_provider);
		}
	}
}
