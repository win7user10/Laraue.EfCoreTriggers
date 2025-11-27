using System.Linq;
using Laraue.Triggers.Core.SqlGeneration;
using Laraue.Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Triggers.Core.Converters.MethodCall.String.Concat
{
    /// <inheritdoc />
    public class ConcatStringViaDoubleVerticalLineVisitor : BaseStringConcatVisitor
    {
        /// <inheritdoc />
        public ConcatStringViaDoubleVerticalLineVisitor(IExpressionVisitorFactory visitorFactory)
            : base(visitorFactory)
        {
        }
        
        /// <inheritdoc />
        protected override SqlBuilder Visit(SqlBuilder[] argumentsSql)
        {
            return new SqlBuilder()
                .AppendJoin(" || ", argumentsSql.Select(x => x.ToString()));
        }
    }
}