using Laraue.EfCoreTriggers.Common.Builders.Triggers.OnDelete;
using Laraue.EfCoreTriggers.Common.Builders.Triggers.OnInsert;
using Laraue.EfCoreTriggers.Common.Builders.Visitor;
using Laraue.EfCoreTriggers.Tests.Entities;
using Laraue.EfCoreTriggers.Tests.Enums;
using Moq;
using System;
using System.Linq.Expressions;
using Xunit;

namespace Laraue.EfCoreTriggers.Tests.SqlGeneration
{
    public abstract class BaseSetExpressionsTests
    {
        protected readonly ITriggerSqlVisitor Visitor;

        public BaseSetExpressionsTests(ITriggerSqlVisitor visitor)
        {
            Visitor = visitor;
        }

        protected OnInsertTriggerInsertAction<Transaction, TransactionMirror> NewInsertOnInsertAction(Expression<Func<Transaction, TransactionMirror>> setValues)
            => new OnInsertTriggerInsertAction<Transaction, TransactionMirror>(setValues);

        public abstract void StringLowerSql();

        public abstract void StringUpperSql();

        public abstract void StringConcatSql();

        [Fact]
        public virtual void EnumValueSql()
        {
            var action = new OnInsertTriggerInsertAction<Transaction, User>(transaction => new User { Role = UserRole.Admin });
            var sql = action.BuildSql(Visitor);
            Assert.Equal("INSERT INTO users (role) VALUES (999)", sql);
        }

        [Fact]
        public virtual void DecimalAddSql()
        {
            var action = new OnInsertTriggerInsertAction<TestEntity, TestEntity>(t => new TestEntity { DecimalValue = t.DecimalValue + 3 });
            var sql = action.BuildSql(Visitor);
            Assert.Equal("INSERT INTO test_entities (decimal_value) VALUES (NEW.decimal_value + 3)", sql);
        }

        [Fact]
        public virtual void DoubleSubSql()
        {
            var action = new OnInsertTriggerInsertAction<TestEntity, TestEntity>(t => new TestEntity { DoubleValue = t.DoubleValue - 3 });
            var sql = action.BuildSql(Visitor);
            Assert.Equal("INSERT INTO test_entities (double_value) VALUES (NEW.double_value - 3)", sql);
        }

        [Fact]
        public virtual void IntMultiplySql()
        {
            var action = new OnInsertTriggerInsertAction<TestEntity, TestEntity>(t => new TestEntity { IntValue = t.IntValue * 2 });
            var sql = action.BuildSql(Visitor);
            Assert.Equal("INSERT INTO test_entities (int_value) VALUES (NEW.int_value * 2)", sql);
        }
    }
}