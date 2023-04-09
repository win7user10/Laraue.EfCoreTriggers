using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.CSharpMethods;
using Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.CSharpMethods
{
    /// <summary>
    /// Base visitor for <see cref="BinaryFunctions"/> methods.
    /// </summary>
    public class CoalesceVisitor : BaseBinaryFunctionsVisitor
    {
        protected override string MethodName => nameof(BinaryFunctions.Coalesce);

        /// <inheritdoc />
        public CoalesceVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }

        public override SqlBuilder Visit(MethodCallExpression expression, VisitedMembers visitedMembers)
        {
            var argumentsSql = expression.Arguments
                .Select(argument => VisitorFactory.Visit(argument, visitedMembers))
                .ToArray();
            
            return GetSql(argumentsSql[0], argumentsSql[1]);
        }

        protected virtual SqlBuilder GetSql(SqlBuilder isNullExpressionSql, SqlBuilder whenNullExpressionSql)
        {
            return SqlBuilder.FromString($"COALESCE({isNullExpressionSql}, {whenNullExpressionSql})");
        }
    }
}