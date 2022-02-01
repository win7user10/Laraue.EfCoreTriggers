using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Contains
{
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

        protected abstract string CombineSql(string expressionToSearchSql, string expressionToFindSql);
    }
}