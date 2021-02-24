using System;
using Laraue.EfCoreTriggers.Common.Builders.Providers;
using Laraue.EfCoreTriggers.Common.Builders.Triggers.OnInsert;
using Laraue.EfCoreTriggers.Extensions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Laraue.EfCoreTriggers.SqlServerTests.Issues._3
{
	public class Tests : IDisposable
	{
		private const string ExpectedSaleCategoryTriggerSql = "CREATE TRIGGER LC_TRIGGER_AFTER_INSERT_SALESCATEGORY " +
		                                                      "ON PolicySchema.SalesCategory AFTER Insert AS BEGIN " +
		                                                      "DECLARE InsertedSalesCategoryCursor CURSOR " +
		                                                      "FOR SELECT Status, Id FROM Inserted " +
		                                                      "DECLARE @NewStatus INT, @NewId BIGINT " +
		                                                      "OPEN InsertedSalesCategoryCursor " +
		                                                      "FETCH NEXT FROM InsertedSalesCategoryCursor INTO @NewStatus, @NewId WHILE @@FETCH_STATUS = 0 " +
		                                                      "BEGIN IF (@NewStatus = 0) " +
		                                                      "BEGIN UPDATE PolicySchema.SalesCategory SET Status = 1 WHERE @NewId = Id; " +
		                                                      "END " +
		                                                      "FETCH NEXT FROM InsertedSalesCategoryCursor " +
		                                                      "INTO @NewStatus, @NewId END CLOSE InsertedSalesCategoryCursor " +
		                                                      "DEALLOCATE InsertedSalesCategoryCursor " +
		                                                      "END";

		private const string ExpectedSalesAreaTriggerSql = "CREATE TRIGGER LC_TRIGGER_AFTER_INSERT_SALESAREA " +
		                                                   "ON SalesArea AFTER Insert AS BEGIN " +
		                                                   "DECLARE InsertedSalesAreaCursor CURSOR " +
		                                                   "FOR SELECT Status, Id FROM Inserted " +
		                                                   "DECLARE @NewStatus INT, @NewId BIGINT " +
		                                                   "OPEN InsertedSalesAreaCursor " +
		                                                   "FETCH NEXT FROM InsertedSalesAreaCursor INTO @NewStatus, @NewId WHILE @@FETCH_STATUS = 0 " +
		                                                   "BEGIN IF (@NewStatus = 0) " +
		                                                   "BEGIN UPDATE SalesArea SET Status = 1 WHERE @NewId = Id; " +
		                                                   "END " +
		                                                   "FETCH NEXT FROM InsertedSalesAreaCursor " +
		                                                   "INTO @NewStatus, @NewId END CLOSE InsertedSalesAreaCursor " +
		                                                   "DEALLOCATE InsertedSalesAreaCursor END";

		private readonly ITriggerProvider _provider;
		private readonly DbContext _context;

		public Tests()
		{
			_context = new TestDbContext(new DbContextOptionsBuilder<TestDbContext>()
				.UseSqlServer()
				.UseTriggers()
				.Options);

			_provider = new SqlServerProvider(_context.Model); 
		}

		[Fact]
		public virtual void SalesCategoryShouldBeGeneratedCorrectSql()
		{
			var trigger = new OnInsertTrigger<SalesCategory>(TriggerTime.After)
				.Action(action => action
					.Condition(f => f.Status == EntityStatus.New)
					.Update<SalesCategory>((a, b) => a.Id == b.Id, (a, b) => new SalesCategory() { Status = EntityStatus.Draft }));

			var sql = trigger.BuildSql(_provider);
			Assert.Equal(ExpectedSaleCategoryTriggerSql, sql);
		}

		[Fact]
		public virtual void SalesAreaShouldBeGeneratedCorrectSql()
		{
			var trigger = new OnInsertTrigger<SalesArea>(TriggerTime.After)
				.Action(action => action
					.Condition(f => f.Status == EntityStatus.New)
					.Update<SalesArea>((a, b) => a.Id == b.Id, (a, b) => new SalesArea() { Status = EntityStatus.Draft }));

			var sql = trigger.BuildSql(_provider);
			Assert.Equal(ExpectedSalesAreaTriggerSql, sql);
		}


		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
