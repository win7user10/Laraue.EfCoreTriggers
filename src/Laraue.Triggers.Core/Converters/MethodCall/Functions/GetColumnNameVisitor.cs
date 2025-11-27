using System.Linq.Expressions;
using Laraue.Triggers.Core.Functions;
using Laraue.Triggers.Core.SqlGeneration;
using Laraue.Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Triggers.Core.Converters.MethodCall.Functions
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