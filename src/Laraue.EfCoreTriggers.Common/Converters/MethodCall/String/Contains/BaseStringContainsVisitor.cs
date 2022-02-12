using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Contains
{
    /// <summary>
    /// Visitor for <see cref="System.String.Contains(string)"/> method.
    /// </summary>
    public abstract class BaseStringContainsVisitor : BaseStringVisitor
    {
        protected BaseStringContainsVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }
        
        /// <inheritdoc />
        protected override string MethodName => nameof(string.Contains);

        /// <inheritdoc />
        public override SqlBuilder Visit(
            MethodCallExpression expression,
            ArgumentTypes argumentTypes,
            VisitedMembers visitedMembers)
        {
            var expressionToFindSql = VisitorFactory.VisitArguments(expression, argumentTypes, visitedMembers)[0];
            var expressionToSearchSql = VisitorFactory.Visit(expression.Object, argumentTypes, visitedMembers);
            
            return SqlBuilder.FromString(CombineSql(expressionToSearchSql, expressionToFindSql));
        }

        /// <summary>
        /// Build contains SQL expression.
        /// </summary>
        /// <param name="expressionToSearchSql">The search string SQL.</param>
        /// <param name="expressionToFindSql">Where to search the string SQL.</param>
        /// <returns></returns>
        protected abstract string CombineSql(string expressionToSearchSql, string expressionToFindSql);
    }
}