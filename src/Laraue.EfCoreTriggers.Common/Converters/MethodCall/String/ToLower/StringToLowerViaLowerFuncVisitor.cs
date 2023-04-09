using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.ToLower
{
    public class StringToLowerViaLowerFuncVisitor : BaseStringVisitor
    {
        /// <inheritdoc />
        protected override string MethodName => nameof(string.ToLower);
        
        /// <inheritdoc />
        public StringToLowerViaLowerFuncVisitor(IExpressionVisitorFactory visitorFactory)
            : base(visitorFactory)
        {
        }

        /// <inheritdoc />
        public override SqlBuilder Visit(
            MethodCallExpression expression,
            VisitedMembers visitedMembers)
        {
            var sqlBuilder = VisitorFactory.Visit(expression.Object, visitedMembers);
            
            return SqlBuilder.FromString($"LOWER({sqlBuilder})");
        }
    }
}