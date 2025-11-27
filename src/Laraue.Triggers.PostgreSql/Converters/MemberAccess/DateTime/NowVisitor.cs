using System.Linq.Expressions;
using Laraue.Triggers.Core.Converters.MemberAccess.DateTime;
using Laraue.Triggers.Core.SqlGeneration;
using Laraue.Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Triggers.PostgreSql.Converters.MemberAccess.DateTime
{
    /// <inheritdoc />
    public class NowVisitor : BaseNowVisitor
    {
        /// <inheritdoc />
        public NowVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }

        /// <inheritdoc />
        public override SqlBuilder Visit(MemberExpression expression)
        {
            return SqlBuilder.FromString("NOW()");
        }
    }
}
