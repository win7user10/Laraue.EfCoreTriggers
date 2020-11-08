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
    }
}
