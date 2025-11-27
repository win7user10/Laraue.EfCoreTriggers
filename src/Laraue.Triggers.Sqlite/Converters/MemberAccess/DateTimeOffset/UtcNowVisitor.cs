using System.Linq.Expressions;
using Laraue.Triggers.Core.SqlGeneration;
using Laraue.Triggers.Core.Visitors.ExpressionVisitors;
using BaseUtcNowVisitor = Laraue.Triggers.Core.Converters.MemberAccess.DateTimeOffset.BaseUtcNowVisitor;

namespace Laraue.Triggers.Sqlite.Converters.MemberAccess.DateTimeOffset
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
