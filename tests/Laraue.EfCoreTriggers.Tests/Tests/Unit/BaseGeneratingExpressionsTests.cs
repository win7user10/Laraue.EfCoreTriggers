using System;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.OnInsert;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;
using Laraue.EfCoreTriggers.Tests.Entities;
using Laraue.EfCoreTriggers.Tests.Enums;
using Xunit;

namespace Laraue.EfCoreTriggers.Tests.Tests.Unit
{
    public abstract class BaseGeneratingExpressionsTests
    {
        protected readonly ITriggerActionVisitorFactory Factory;

        protected BaseGeneratingExpressionsTests(ITriggerActionVisitorFactory factory)
        {
            Factory = factory;
        }

        protected abstract string ExceptedConcatSql { get; }

        protected abstract string ExceptedStringLowerSql { get; }

        protected abstract string ExceptedStringUpperSql { get; }

        public abstract string ExceptedEnumValueSql { get; }

        public abstract string ExceptedDecimalAddSql { get; }

        public abstract string ExceptedDoubleSubSql { get; }

        public abstract string ExceptedIntMultiplySql { get; }

        public abstract string ExceptedBooleanSql { get; }

        public abstract string ExceptedNewGuidSql { get; }

        public abstract string ExceptedStringTrimSql { get; }

        public abstract string ExceptedContainsSql { get; }

        public abstract string ExceptedEndsWithSql { get; }

        public abstract string ExceptedIsNullOrEmptySql { get; }

        public abstract string ExceptedAbsSql { get; }

        public abstract string ExceptedAcosSql { get; }

        public abstract string ExceptedAsinSql { get; }

        public abstract string ExceptedAtanSql { get; }

        public abstract string ExceptedAtanTwoSql { get; }

        public abstract string ExceptedCeilingSql { get; }

        public abstract string ExceptedCosSql { get; }

        public abstract string ExceptedExpSql { get; }

        public abstract string ExceptedFloorSql { get; }

        [Fact]
        public virtual void StringConcatSql()
        {
            var action = new OnInsertTriggerInsertAction<Transaction, TransactionMirror>(transaction =>
                new TransactionMirror
                {
                    Description = transaction.Description + "abc",
                });
            
            var sql = Factory.Visit(action, new VisitedMembers());
            
            Assert.Equal(ExceptedConcatSql, sql);
        }

        [Fact]
        public virtual void StringLowerSql()
        {
            var action = new OnInsertTriggerInsertAction<Transaction, TransactionMirror>(transaction => new TransactionMirror
            {
                Description = transaction.Description.ToLower()
            });
            
            var sql = Factory.Visit(action, new VisitedMembers());
            
            Assert.Equal(ExceptedStringLowerSql, sql);
        }

        [Fact]
        public virtual void StringUpperSql()
        {
            var action = new OnInsertTriggerInsertAction<Transaction, TransactionMirror>(
                transaction => new TransactionMirror
                {
                    Description = transaction.Description.ToUpper()
                });
            
            var sql = Factory.Visit(action, new VisitedMembers());
            
            Assert.Equal(ExceptedStringUpperSql, sql);
        }

        [Fact]
        public virtual void EnumValueSql()
        {
            var action = new OnInsertTriggerInsertAction<Transaction, User>(
                transaction => new User
                {
                    Role = UserRole.Admin
                });
            
            var sql = Factory.Visit(action, new VisitedMembers());
            
            Assert.Equal(ExceptedEnumValueSql, sql);
        }

        [Fact]
        public virtual void DecimalAddSql()
        {
            var action = new OnInsertTriggerInsertAction<TestEntity, TestEntity>(
                t => new TestEntity
                {
                    DecimalValue = t.DecimalValue + 3
                });
            
            var sql = Factory.Visit(action, new VisitedMembers());
            
            Assert.Equal(ExceptedDecimalAddSql, sql);
        }

        [Fact]
        public virtual void DoubleSubSql()
        {
            var action = new OnInsertTriggerInsertAction<TestEntity, TestEntity>(
                t => new TestEntity
                {
                    DoubleValue = t.DoubleValue - 3
                });
            
            var sql = Factory.Visit(action, new VisitedMembers());
            
            Assert.Equal(ExceptedDoubleSubSql, sql);
        }

        [Fact]
        public virtual void IntMultiplySql()
        {
            var action = new OnInsertTriggerInsertAction<TestEntity, TestEntity>(
                t => new TestEntity
                {
                    IntValue = t.IntValue * 2
                });
            
            var sql = Factory.Visit(action, new VisitedMembers());
            
            Assert.Equal(ExceptedIntMultiplySql, sql);
        }

        [Fact]
        public virtual void BooleanValueSql()
        {
            var action = new OnInsertTriggerInsertAction<TestEntity, TestEntity>(
                t => new TestEntity
                {
                    BooleanValue = true
                });
            
            var sql = Factory.Visit(action, new VisitedMembers());
            
            Assert.Equal(ExceptedBooleanSql, sql);
        }

        [Fact]
        public virtual void NewGuid()
        {
            var action = new OnInsertTriggerInsertAction<TestEntity, TestEntity>(
                t => new TestEntity
                {
                    GuidValue = new Guid()
                });
            
            var sql = Factory.Visit(action, new VisitedMembers());
            
            Assert.Equal(ExceptedNewGuidSql, sql);
        }

        [Fact]
        public virtual void StringTrimSql()
        {
            var action = new OnInsertTriggerInsertAction<Transaction, TransactionMirror>(
                transaction => new TransactionMirror
                {
                    Description = transaction.Description.Trim()
                });
            
            var sql = Factory.Visit(action, new VisitedMembers());
            
            Assert.Equal(ExceptedStringTrimSql, sql);
        }

        [Fact]
        public virtual void StringContainsSql()
        {
            var action = new OnInsertTriggerInsertAction<Transaction, TransactionMirror>(transaction => new TransactionMirror
            {
                IsVeryfied = transaction.Description.Contains("abc"),
            });
            
            var sql = Factory.Visit(action, new VisitedMembers());
            
            Assert.Equal(ExceptedContainsSql, sql);
        }

        [Fact]
        public virtual void StringEndsWithSql()
        {
            var action = new OnInsertTriggerInsertAction<Transaction, TransactionMirror>(transaction => new TransactionMirror
            {
                IsVeryfied = transaction.Description.EndsWith("abc"),
            });
            
            var sql = Factory.Visit(action, new VisitedMembers());
            
            Assert.Equal(ExceptedEndsWithSql, sql);
        }

        [Fact]
        public virtual void StringIsNullOrEmptySql()
        {
            var action = new OnInsertTriggerInsertAction<Transaction, TransactionMirror>(transaction => new TransactionMirror
            {
                IsVeryfied = string.IsNullOrEmpty(transaction.Description),
            });
            
            var sql = Factory.Visit(action, new VisitedMembers());
            
            Assert.Equal(ExceptedIsNullOrEmptySql, sql);
        }

        [Fact]
        public virtual void MathAbsSql()
        {
            var action = new OnInsertTriggerInsertAction<Transaction, TransactionMirror>(transaction => new TransactionMirror
            {
                Value = Math.Abs(transaction.Value),
            });
            
            var sql = Factory.Visit(action, new VisitedMembers());
            
            Assert.Equal(ExceptedAbsSql, sql);
        }

        [Fact]
        public virtual void MathAcosSql()
        {
            var action = new OnInsertTriggerInsertAction<Transaction, TransactionMirror>(transaction => new TransactionMirror
            {
                DoubleValue = Math.Acos(transaction.DoubleValue),
            });
            
            var sql = Factory.Visit(action, new VisitedMembers());
            
            Assert.Equal(ExceptedAcosSql, sql);
        }

        [Fact]
        public virtual void MathAsinSql()
        {
            var action = new OnInsertTriggerInsertAction<Transaction, TransactionMirror>(transaction => new TransactionMirror
            {
                DoubleValue = Math.Asin(transaction.DoubleValue),
            });
            
            var sql = Factory.Visit(action, new VisitedMembers());
            
            Assert.Equal(ExceptedAsinSql, sql);
        }

        [Fact]
        public virtual void MathAtanSql()
        {
            var action = new OnInsertTriggerInsertAction<Transaction, TransactionMirror>(transaction => new TransactionMirror
            {
                DoubleValue = Math.Atan(transaction.DoubleValue),
            });
            
            var sql = Factory.Visit(action, new VisitedMembers());
            
            Assert.Equal(ExceptedAtanSql, sql);
        }

        [Fact]
        public virtual void MathAtanTwoSql()
        {
            var action = new OnInsertTriggerInsertAction<Transaction, TransactionMirror>(transaction => new TransactionMirror
            {
                DoubleValue = Math.Atan2(transaction.DoubleValue, transaction.DoubleValue),
            });
            
            var sql = Factory.Visit(action, new VisitedMembers());
            
            Assert.Equal(ExceptedAtanTwoSql, sql);
        }

        [Fact]
        public virtual void MathCeilingTwoSql()
        {
            var action = new OnInsertTriggerInsertAction<Transaction, TransactionMirror>(transaction => new TransactionMirror
            {
                DoubleValue = Math.Ceiling(transaction.DoubleValue),
            });
            
            var sql = Factory.Visit(action, new VisitedMembers());
            
            Assert.Equal(ExceptedCeilingSql, sql);
        }

        [Fact]
        public virtual void MathCosTwoSql()
        {
            var action = new OnInsertTriggerInsertAction<Transaction, TransactionMirror>(transaction => new TransactionMirror
            {
                DoubleValue = Math.Cos(transaction.DoubleValue),
            });
            
            var sql = Factory.Visit(action, new VisitedMembers());
            
            Assert.Equal(ExceptedCosSql, sql);
        }

        [Fact]
        public virtual void MathExpTwoSql()
        {
            var action = new OnInsertTriggerInsertAction<Transaction, TransactionMirror>(transaction => new TransactionMirror
            {
                DoubleValue = Math.Exp(transaction.DoubleValue),
            });
            
            var sql = Factory.Visit(action, new VisitedMembers());
            
            Assert.Equal(ExceptedExpSql, sql);
        }

        [Fact]
        public virtual void MathFloorTwoSql()
        {
            var action = new OnInsertTriggerInsertAction<Transaction, TransactionMirror>(transaction => new TransactionMirror
            {
                DoubleValue = Math.Floor(transaction.DoubleValue),
            });
            
            var sql = Factory.Visit(action, new VisitedMembers());
            
            Assert.Equal(ExceptedFloorSql, sql);
        }
    }
}