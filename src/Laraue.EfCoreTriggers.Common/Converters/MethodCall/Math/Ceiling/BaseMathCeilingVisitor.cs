using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Ceiling
{
    /// <summary>
    /// Visitor for <see cref="System.Math.Ceiling(decimal)"/> method.
    /// </summary>
    public abstract class BaseMathCeilingVisitor : BaseMathVisitor
    {
        /// <inheritdoc />
        protected override string MethodName => nameof(System.Math.Ceiling);

        /// <inheritdoc />
        protected BaseMathCeilingVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }

        /// <summary>
        /// Ceil function name in SQL.
        /// </summary>
        protected abstract string SqlFunctionName { get; }

        /// <inheritdoc />
        public override SqlBuilder Visit(
            MethodCallExpression expression,
            ArgumentTypes argumentTypes,
            VisitedMembers visitedMembers)
        {
            var argument = expression.Arguments[0];
            
            var sqlBuilder = VisitorFactory.Visit(argument, argumentTypes, visitedMembers);
            
            return SqlBuilder.FromString($"{SqlFunctionName}({sqlBuilder})");
        }
    }
}
