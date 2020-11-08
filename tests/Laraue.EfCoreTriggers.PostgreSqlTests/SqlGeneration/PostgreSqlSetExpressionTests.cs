using Laraue.EfCoreTriggers.Common.Builders.Providers;
using Laraue.EfCoreTriggers.Common.Builders.Triggers.OnInsert;
using Laraue.EfCoreTriggers.Tests.Entities;
using Laraue.EfCoreTriggers.Tests.SqlGeneration;
using Xunit;

namespace Laraue.EfCoreTriggers.PostgreSqlTests.SqlGeneration
{
    public class PostgreSqlSetExpressionTests : BaseSetExpressionsTests
    {
        public PostgreSqlSetExpressionTests() : base(new PostgreSqlVisitor(new ContextFactory().CreatePgDbContext().Model))
        {
        }

        [Fact]
        public override void StringConcatSql()
        {
            var sql = new OnInsertTriggerInsertAction<Transaction, TransactionMirror>(transaction => new TransactionMirror
            {
                Description = transaction.Description + "abc",
            }).BuildSql(Visitor);
            Assert.Equal("INSERT INTO transactions_mirror (description) VALUES (CONCAT(NEW.description, 'abc'))", sql);
        }

        [Fact]
        public override void StringLowerSql()
        {
            var sql = new OnInsertTriggerInsertAction<Transaction, TransactionMirror>(transaction => new TransactionMirror
            {
                Description = transaction.Description.ToLower()
            }).BuildSql(Visitor);
            Assert.Equal("INSERT INTO transactions_mirror (description) VALUES (LOWER(NEW.description))", sql);
        }

        [Fact]
        public override void StringUpperSql()
        {
            var sql = new OnInsertTriggerInsertAction<Transaction, TransactionMirror>(transaction => new TransactionMirror
            {
                Description = transaction.Description.ToUpper()
            }).BuildSql(Visitor);
            Assert.Equal("INSERT INTO transactions_mirror (description) VALUES (UPPER(NEW.description))", sql);
        }
    }
}
