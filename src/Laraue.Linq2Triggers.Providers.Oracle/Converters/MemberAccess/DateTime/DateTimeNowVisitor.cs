using System.Linq.Expressions;
using Laraue.Linq2Triggers.Core.Converters.MemberAccess.DateTime;
using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Linq2Triggers.Providers.Oracle.Converters.MemberAccess.DateTime
{
    /// <inheritdoc />
    public class DateTimeNowVisitor : BaseNowVisitor
    {
        /// <inheritdoc />
        public DateTimeNowVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }

        /// <inheritdoc />
        public override SqlBuilder Visit(MemberExpression expression)
        {
            return SqlBuilder.FromString("CURRENT_DATE");
        }
    }
}
