﻿using Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Ceiling
{
    /// <inheritdoc />
    public class MathCeilVisitor : BaseMathCeilingVisitor
    {
        /// <inheritdoc />
        public MathCeilVisitor(IExpressionVisitorFactory visitorFactory) : base(visitorFactory)
        {
        }

        /// <inheritdoc />
        protected override string SqlFunctionName => "CEIL";
    }
}
