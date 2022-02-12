using System.Linq;
using Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Concat
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