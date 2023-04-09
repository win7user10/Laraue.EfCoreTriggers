using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Xunit;

namespace Laraue.EfCoreTriggers.Tests.Tests.Base
{
    /// <summary>
    /// Tests of translating <see cref="Math"/> functions to SQL code.
    /// </summary>
    public abstract class BaseMathFunctionsTests
    {
        /// <summary>
        /// DecimalValue = Math.Abs(NEW.DecimalValue)
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> MathAbsDecimalValueExpression =
            tableRefs => new DestinationEntity
            {
                DecimalValue = Math.Abs(tableRefs.New.DecimalValue)
            };

        [Fact]
        public abstract void MathAbsDecimalSql();

        /// <summary>
        /// DoubleValue = Math.Acos(NEW.DoubleValue)
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> MathAcosDoubleValueExpression =
            tableRefs => new DestinationEntity
            {
                DoubleValue = Math.Acos(tableRefs.New.DoubleValue)
            };

        [Fact]
        public abstract void MathAcosSql();

        /// <summary>
        /// DoubleValue = Math.Asin(NEW.DoubleValue)
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> MathAsinDoubleValueExpression =
            tableRefs => new DestinationEntity
            {
                DoubleValue = Math.Asin(tableRefs.New.DoubleValue)
            };

        [Fact]
        public abstract void MathAsinSql();

        /// <summary>
        /// DoubleValue = Math.Atan(NEW.DoubleValue)
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> MathAtanDoubleValueExpression =
            tableRefs => new DestinationEntity
            {
                DoubleValue = Math.Atan(tableRefs.New.DoubleValue)
            };

        [Fact]
        public abstract void MathAtanSql();

        /// <summary>
        /// DoubleValue = Math.Atan2(NEW.DoubleValue, NEW.DoubleValue)
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> MathAtan2DoubleValueExpression =
            tableRefs => new DestinationEntity
            {
                DoubleValue = Math.Atan2(tableRefs.New.DoubleValue, tableRefs.New.DoubleValue)
            };

        [Fact]
        public abstract void MathAtan2Sql();

        /// <summary>
        /// DoubleValue = Math.Ceiling(NEW.DoubleValue)
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> MathCeilingDoubleValueExpression =
            tableRefs => new DestinationEntity
            {
                DoubleValue = Math.Ceiling(tableRefs.New.DoubleValue)
            };

        [Fact]
        public abstract void MathCeilingDoubleSql();

        /// <summary>
        /// DoubleValue = Math.Cos(NEW.DoubleValue)
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> MathCosDoubleValueExpression =
            tableRefs => new DestinationEntity
            {
                DoubleValue = Math.Cos(tableRefs.New.DoubleValue)
            };

        [Fact]
        public abstract void MathCosSql();

        /// <summary>
        /// DoubleValue = Math.Exp(NEW.DoubleValue)
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> MathExpDoubleValueExpression =
            tableRefs => new DestinationEntity
            {
                DoubleValue = Math.Exp(tableRefs.New.DoubleValue)
            };

        [Fact]
        public abstract void MathExpSql();

        /// <summary>
        /// DoubleValue = Math.Floor(NEW.DoubleValue)
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> MathFloorDoubleValueExpression =
            tableRefs => new DestinationEntity
            {
                DoubleValue = Math.Floor(tableRefs.New.DoubleValue)
            };

        [Fact]
        public abstract void MathFloorDoubleSql();
    }
}