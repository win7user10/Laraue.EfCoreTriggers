using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Converters.MemberAccess.DateTime;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.SqlServer.Converters.MemberAccess.DateTime
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
            return SqlBuilder.FromString("SYSDATETIME()");
        }
    }
}
