using System.Linq.Expressions;
using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;
using DateTimeOffset_BaseUtcNowVisitor = Laraue.Linq2Triggers.Core.Converters.MemberAccess.DateTimeOffset.BaseUtcNowVisitor;

namespace Laraue.Linq2Triggers.Providers.Sqlite.Converters.MemberAccess.DateTimeOffset
{
    /// <inheritdoc />
    public class UtcNowVisitor : DateTimeOffset_BaseUtcNowVisitor
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
