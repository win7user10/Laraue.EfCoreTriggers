using Laraue.EfCoreTriggers.Tests.Entities;
using Laraue.EfCoreTriggers.Tests.Enums;
using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.OnInsert;
using Xunit;

namespace Laraue.EfCoreTriggers.Tests
{
    public abstract class BaseGeneratingExpressionsTests
    {
        protected readonly ITriggerProvider Visitor;

        public BaseGeneratingExpressionsTests(ITriggerProvider visitor)
        {
            Visitor = visitor;
        }

        protected static OnInsertTriggerInsertAction<Transaction, TransactionMirror> NewInsertOnInsertAction(Expression<Func<Transaction, TransactionMirror>> setValues)
            => new(setValues);

        public abstract string ExceptedConcatSql { get; }

        public abstract string ExceptedStringLowerSql { get; }

        public abstract string ExceptedStringUpperSql { get; }

        public abstract string ExceptedEnumValueSql { get; }

        public abstract string ExceptedDecimalAddSql { get; }

        public abstract string ExceptedDoubleSubSql { get; }

        public abstract string ExceptedIntMultiplySql { get; }

        public abstract string ExceptedBooleanSql { get; }

        public abstract string ExceptedNewGuidSql { get; }

        [Fact]
        public virtual void StringConcatSql()
        {
            var sql = new OnInsertTriggerInsertAction<Transaction, TransactionMirror>(transaction => new TransactionMirror
            {
                Description = transaction.Description + "abc",
            }).BuildSql(Visitor);
            Assert.Equal(ExceptedConcatSql, sql);
        }

        [Fact]
        public virtual void StringLowerSql()
        {
            var sql = new OnInsertTriggerInsertAction<Transaction, TransactionMirror>(transaction => new TransactionMirror
            {
                Description = transaction.Description.ToLower()
            }).BuildSql(Visitor);
            Assert.Equal(ExceptedStringLowerSql, sql);
        }

        [Fact]
        public virtual void StringUpperSql()
        {
            var sql = new OnInsertTriggerInsertAction<Transaction, TransactionMirror>(transaction => new TransactionMirror
            {
                Description = transaction.Description.ToUpper()
            }).BuildSql(Visitor);
            Assert.Equal(ExceptedStringUpperSql, sql);
        }

        [Fact]
        public virtual void EnumValueSql()
        {
            var action = new OnInsertTriggerInsertAction<Transaction, User>(transaction => new User { Role = UserRole.Admin });
            var sql = action.BuildSql(Visitor);
            Assert.Equal(ExceptedEnumValueSql, sql);
        }

        [Fact]
        public virtual void DecimalAddSql()
        {
            var action = new OnInsertTriggerInsertAction<TestEntity, TestEntity>(t => new TestEntity { DecimalValue = t.DecimalValue + 3 });
            var sql = action.BuildSql(Visitor);
            Assert.Equal(ExceptedDecimalAddSql, sql);
        }

        [Fact]
        public virtual void DoubleSubSql()
        {
            var action = new OnInsertTriggerInsertAction<TestEntity, TestEntity>(t => new TestEntity { DoubleValue = t.DoubleValue - 3 });
            var sql = action.BuildSql(Visitor);
            Assert.Equal(ExceptedDoubleSubSql, sql);
        }

        [Fact]
        public virtual void IntMultiplySql()
        {
            var action = new OnInsertTriggerInsertAction<TestEntity, TestEntity>(t => new TestEntity { IntValue = t.IntValue * 2 });
            var sql = action.BuildSql(Visitor);
            Assert.Equal(ExceptedIntMultiplySql, sql);
        }

        [Fact]
        public virtual void BooleanValueSql()
        {
            var action = new OnInsertTriggerInsertAction<TestEntity, TestEntity>(t => new TestEntity { BooleanValue = true });
            var sql = action.BuildSql(Visitor);
            Assert.Equal(ExceptedBooleanSql, sql);
        }

        [Fact]
        public virtual void NewGuid()
        {
            var action = new OnInsertTriggerInsertAction<TestEntity, TestEntity>(t => new TestEntity { GuidValue = new Guid() });
            var sql = action.BuildSql(Visitor);
            Assert.Equal(ExceptedNewGuidSql, sql);
        }
    }
}