using System;
using System.Linq.Expressions;
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
        /// DecimalValue = Math.Abs(OLD.DecimalValue)
        /// </summary>
        protected Expression<Func<SourceEntity, DestinationEntity>> MathAbsDecimalValueExpression = sourceEntity => new DestinationEntity
        {
            DecimalValue = Math.Abs(sourceEntity.DecimalValue)
        };

        [Fact]
        public abstract void MathAbsDecimalSql();

        /// <summary>
        /// DoubleValue = Math.Acos(OLD.DoubleValue)
        /// </summary>
        protected Expression<Func<SourceEntity, DestinationEntity>> MathAcosDoubleValueExpression = sourceEntity => new DestinationEntity
        {
            DoubleValue = Math.Acos(sourceEntity.DoubleValue)
        };

        [Fact]
        public abstract void MathAcosSql();

        /// <summary>
        /// DoubleValue = Math.Asin(OLD.DoubleValue)
        /// </summary>
        protected Expression<Func<SourceEntity, DestinationEntity>> MathAsinDoubleValueExpression = sourceEntity => new DestinationEntity
        {
            DoubleValue = Math.Asin(sourceEntity.DoubleValue)
        };

        [Fact]
        public abstract void MathAsinSql();

        /// <summary>
        /// DoubleValue = Math.Atan(OLD.DoubleValue)
        /// </summary>
        protected Expression<Func<SourceEntity, DestinationEntity>> MathAtanDoubleValueExpression = sourceEntity => new DestinationEntity
        {
            DoubleValue = Math.Atan(sourceEntity.DoubleValue)
        };

        [Fact]
        public abstract void MathAtanSql();

        /// <summary>
        /// DoubleValue = Math.Atan2(OLD.DoubleValue, OLD.DoubleValue)
        /// </summary>
        protected Expression<Func<SourceEntity, DestinationEntity>> MathAtan2DoubleValueExpression = sourceEntity => new DestinationEntity
        {
            DoubleValue = Math.Atan2(sourceEntity.DoubleValue, sourceEntity.DoubleValue)
        };

        [Fact]
        public abstract void MathAtan2Sql();

        /// <summary>
        /// DoubleValue = Math.Ceiling(OLD.DoubleValue)
        /// </summary>
        protected Expression<Func<SourceEntity, DestinationEntity>> MathCeilingDoubleValueExpression = sourceEntity => new DestinationEntity
        {
            DoubleValue = Math.Ceiling(sourceEntity.DoubleValue)
        };

        [Fact]
        public abstract void MathCeilingDoubleSql();

        /// <summary>
        /// DoubleValue = Math.Cos(OLD.DoubleValue)
        /// </summary>
        protected Expression<Func<SourceEntity, DestinationEntity>> MathCosDoubleValueExpression = sourceEntity => new DestinationEntity
        {
            DoubleValue = Math.Cos(sourceEntity.DoubleValue)
        };

        [Fact]
        public abstract void MathCosSql();

        /// <summary>
        /// DoubleValue = Math.Exp(OLD.DoubleValue)
        /// </summary>
        protected Expression<Func<SourceEntity, DestinationEntity>> MathExpDoubleValueExpression = sourceEntity => new DestinationEntity
        {
            DoubleValue = Math.Exp(sourceEntity.DoubleValue)
        };

        [Fact]
        public abstract void MathExpSql();

        /// <summary>
        /// DoubleValue = Math.Floor(OLD.DoubleValue)
        /// </summary>
        protected Expression<Func<SourceEntity, DestinationEntity>> MathFloorDoubleValueExpression = sourceEntity => new DestinationEntity
        {
            DoubleValue = Math.Floor(sourceEntity.DoubleValue)
        };

        [Fact]
        public abstract void MathFloorDoubleSql();
    }
}