using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Functions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Functions
{
    /// <summary>
    /// Translates <see cref="TriggerFunctions"/> methods SQL.
    /// </summary>
    public sealed class GetColumnNameVisitor : BaseTriggerFunctionsVisitor
    {
        /// Initializes a new instance of <see cref="GetColumnNameVisitor"/>.
        public GetColumnNameVisitor(IExpressionVisitorFactory visitorFactory)
            : base(visitorFactory)
        {
        }

        /// <inheritdoc />
        protected override string MethodName => nameof(TriggerFunctions.GetColumnName);
    
        /// <inheritdoc />
        public override SqlBuilder Visit(
            MethodCallExpression expression,
            VisitedMembers visitedMembers)
        {
            return VisitorFactory.Visit(expression.Arguments[0], visitedMembers);
        }
    }
}