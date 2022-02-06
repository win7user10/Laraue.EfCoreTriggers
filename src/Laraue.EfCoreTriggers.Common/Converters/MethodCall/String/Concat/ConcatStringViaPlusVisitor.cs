using System.Linq;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Concat
{
    public class ConcatStringViaPlusVisitor : BaseStringConcatVisitor
    {
        public ConcatStringViaPlusVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }
        
        /// <inheritdoc />
        protected override SqlBuilder Visit(SqlBuilder[] argumentsSql)
        {
            return new SqlBuilder()
                .AppendJoin(" + ", argumentsSql.Select(x => x.ToString()));
        }
    }
}