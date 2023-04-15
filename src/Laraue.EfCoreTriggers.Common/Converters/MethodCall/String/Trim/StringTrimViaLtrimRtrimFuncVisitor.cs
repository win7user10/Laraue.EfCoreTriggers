using System.Collections.Generic;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Trim
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