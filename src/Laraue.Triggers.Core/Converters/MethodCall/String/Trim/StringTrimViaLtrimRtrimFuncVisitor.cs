using System.Collections.Generic;
using Laraue.Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Triggers.Core.Converters.MethodCall.String.Trim
{
    /// <inheritdoc />
    public class StringTrimViaLtrimRtrimFuncVisitor : BaseStringTrimVisitor
    {
        /// <inheritdoc />
        protected override IEnumerable<string> SqlTrimFunctionsNamesToApply { get; } = new []
        {
            "LTRIM",
            "RTRIM"
        };
        
        /// <inheritdoc />
        public StringTrimViaLtrimRtrimFuncVisitor(IExpressionVisitorFactory visitorFactory)
            : base(visitorFactory)
        {
        }
    }
}