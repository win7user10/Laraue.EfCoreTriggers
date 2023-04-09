using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Enumerable.Count
{
    public class CountVisitor : BaseEnumerableVisitor
    {
        protected override string MethodName => nameof(System.Linq.Enumerable.Count);
    
        private readonly IExpressionVisitorFactory _expressionVisitorFactory;

        public CountVisitor(
            IExpressionVisitorFactory visitorFactory,
            IDbSchemaRetriever schemaRetriever,
            ISqlGenerator sqlGenerator)
            : base(visitorFactory, schemaRetriever, sqlGenerator)
        {
            _expressionVisitorFactory = visitorFactory;
        }
    
        protected override (SqlBuilder, Expression) Visit(Expression[] arguments, VisitedMembers visitedMembers)
        {
            return  (SqlBuilder.FromString("count(*)"), arguments.FirstOrDefault());
        }
    }
}