using System.Collections.Generic;
using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Linq2Triggers.Core.Converters.MethodCall.String.Trim
{
    /// <inheritdoc />
    public class StringTrimViaBtrimFuncVisitor : BaseStringTrimVisitor
    {
        /// <inheritdoc />
        public StringTrimViaBtrimFuncVisitor(IExpressionVisitorFactory visitorFactory)
            : base(visitorFactory)
        {
        }
        
        /// <inheritdoc />
        protected override IEnumerable<string> SqlTrimFunctionsNamesToApply { get; } = new[]
        {
            "BTRIM"
        };
    }
}