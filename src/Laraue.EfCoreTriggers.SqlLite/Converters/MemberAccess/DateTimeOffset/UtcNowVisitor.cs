using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Converters.MemberAccess.DateTimeOffset;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.SqlLite.Converters.MemberAccess.DateTimeOffset
{
    /// <inheritdoc />
    public class UtcNowVisitor : BaseUtcNowVisitor
    {
        /// <inheritdoc />
        public UtcNowVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }

        /// <inheritdoc />
        public override SqlBuilder Visit(MemberExpression expression)
        {
            return SqlBuilder.FromString("DATETIME('now')");
        }
    }
}
