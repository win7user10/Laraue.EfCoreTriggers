using System.Collections.Generic;
using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Trim
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