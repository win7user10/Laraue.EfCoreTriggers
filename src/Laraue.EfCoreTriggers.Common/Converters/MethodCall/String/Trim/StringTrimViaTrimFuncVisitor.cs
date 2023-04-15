using System.Collections.Generic;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Trim
{
    /// <inheritdoc />
    public class StringTrimViaTrimFuncVisitor : BaseStringTrimVisitor
    {
        /// <inheritdoc />
        protected override IEnumerable<string> SqlTrimFunctionsNamesToApply { get; } = new[]
        {
            "TRIM"
        };
        
        /// <inheritdoc />
        public StringTrimViaTrimFuncVisitor(IExpressionVisitorFactory visitorFactory)
            : base(visitorFactory)
        {
        }
    }
}