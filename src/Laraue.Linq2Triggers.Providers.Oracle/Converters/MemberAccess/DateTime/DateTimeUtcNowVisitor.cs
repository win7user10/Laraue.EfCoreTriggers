using System.Linq.Expressions;
using Laraue.Linq2Triggers.Core.Converters.MemberAccess.DateTime;
using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Linq2Triggers.Providers.Oracle.Converters.MemberAccess.DateTime
{
    /// <inheritdoc />
    public class DateTimeUtcNowVisitor : BaseUtcNowVisitor
    {
        /// <inheritdoc />
        public DateTimeUtcNowVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }

        /// <inheritdoc />
        public override SqlBuilder Visit(MemberExpression expression)
        {
            return SqlBuilder.FromString("SYS_EXTRACT_UTC(SYSTIMESTAMP)");
        }
    }
}
