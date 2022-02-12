﻿using Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Ceiling
{
    /// <inheritdoc />
    public class MathCeilingVisitor : BaseMathCeilingVisitor
    {
        /// <inheritdoc />
        public MathCeilingVisitor(IExpressionVisitorFactory visitorFactory) : base(visitorFactory)
        {
        }

        /// <inheritdoc />
        protected override string SqlFunctionName => "CEILING";
    }
}
