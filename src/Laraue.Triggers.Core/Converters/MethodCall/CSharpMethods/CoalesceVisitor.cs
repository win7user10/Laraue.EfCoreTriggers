using System.Linq;
using System.Linq.Expressions;
using Laraue.Triggers.Core.CSharpMethods;
using Laraue.Triggers.Core.SqlGeneration;
using Laraue.Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Triggers.Core.Converters.MethodCall.CSharpMethods
{
    /// <summary>
    /// Base visitor for <see cref="BinaryFunctions"/> methods.
    /// </summary>
    public sealed class CoalesceVisitor : BaseBinaryFunctionsVisitor
    {
        /// <inheritdoc />
        protected override string MethodName => nameof(BinaryFunctions.Coalesce);

        /// <inheritdoc />
        public CoalesceVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }

        /// <inheritdoc />
        public override SqlBuilder Visit(MethodCallExpression expression, VisitedMembers visitedMembers)
        {
            var argumentsSql = expression.Arguments
                .Select(argument => VisitorFactory.Visit(argument, visitedMembers))
                .ToArray();
            
            return GetSql(argumentsSql[0], argumentsSql[1]);
        }

        private static SqlBuilder GetSql(SqlBuilder isNullExpressionSql, SqlBuilder whenNullExpressionSql)
        {
            return SqlBuilder.FromString($"COALESCE({isNullExpressionSql}, {whenNullExpressionSql})");
        }
    }
}