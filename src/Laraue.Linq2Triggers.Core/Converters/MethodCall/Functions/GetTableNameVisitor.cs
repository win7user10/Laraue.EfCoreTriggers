using System.Linq.Expressions;
using Laraue.Linq2Triggers.Core.Functions;
using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Linq2Triggers.Core.Converters.MethodCall.Functions
{
    /// <summary>
    /// Translates <see cref="TriggerFunctions"/> methods SQL.
    /// </summary>
    public sealed class GetTableNameVisitor : BaseTriggerFunctionsVisitor
    {
        private readonly ISqlGenerator _sqlGenerator;

        /// Initializes a new instance of <see cref="GetTableNameVisitor"/>.
        public GetTableNameVisitor(IExpressionVisitorFactory visitorFactory, ISqlGenerator sqlGenerator)
            : base(visitorFactory)
        {
            _sqlGenerator = sqlGenerator;
        }

        /// <inheritdoc />
        protected override string MethodName => nameof(TriggerFunctions.GetTableName);
    
        /// <inheritdoc />
        public override SqlBuilder Visit(
            MethodCallExpression expression,
            VisitedMembers visitedMembers)
        {
            var entityType = expression.Method.GetGenericArguments()[0];

            return new SqlBuilder().Append(_sqlGenerator.GetTableSql(entityType));
        }
    }
}