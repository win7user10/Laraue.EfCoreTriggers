using System.Linq.Expressions;
using Laraue.Linq2Triggers.Core.Converters.MemberAccess.DateTime;
using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Linq2Triggers.Providers.Sqlite.Converters.MemberAccess.DateTime
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
